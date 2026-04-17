
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Dependency;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TestAPI.Data;
using TestAPI.Entities;
using TestAPI.Models;
using TestAPI.Services.Interfaces;
using TestAPI.DTO;
using TestAPI.Persistence.Interfaces;
using Microsoft.IdentityModel.JsonWebTokens;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
namespace TestAPI.Services.Implementation

{
    public class JWTService : IJWTService
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenRepository _tokenRepository;
        private readonly IOptions<AuthSettings> _authSettings;
        private readonly IValidator<RefreshTokenRequest> _refreshTokenRequestValidator;
        public JWTService(IOptions<AuthSettings> authSettings,
        ApplicationDbContext context,
        IConfiguration configuration,
        ITokenRepository tokenRepository,
        IValidator<RefreshTokenRequest> refreshTokenRequestValidator
        )
        {
            _authSettings = authSettings;
            _context = context;
            _tokenRepository = tokenRepository;
            _refreshTokenRequestValidator = refreshTokenRequestValidator;
        }

        public async Task<TokenResponse> ProvideTokens(User user)
        {
            var secretKey = _authSettings.Value.SecretKey;
            if (secretKey == null)
            {
                throw new ArgumentNullException(nameof(secretKey), "Secret key not found");
            }
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.Value.SecretKey!));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName!)
            };

            var jwtToken = new JwtSecurityToken(
                issuer: _authSettings.Value.Issuer,
                audience: _authSettings.Value.Audience,
                expires: DateTime.UtcNow.Add(_authSettings.Value.Expires),
                claims: claims,
                signingCredentials: credentials
                );

            var refreshTokens = GenerateRefreshToken();
            
            var tokens = new TokenResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                RefreshToken = refreshTokens.RawToken
            };

            var existingRefreshToken = await _context.RefreshTokens
            .OrderByDescending(t => t.CreatedAt)
            .FirstOrDefaultAsync(t => t.UserId == user.Id);
            if(existingRefreshToken != null) {
                existingRefreshToken.Revoked = true;
                await _context.SaveChangesAsync();
            }

                var newRefreshToken = new RefreshToken {
                UserId = user.Id,
                TokenHash = refreshTokens.HashToken,
                ExpiresAt = DateTime.UtcNow.AddDays(14),
                Revoked = false
            };
            await _context.RefreshTokens.AddAsync(newRefreshToken);
            await _context.SaveChangesAsync();
            
            return tokens;
        }

        public async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request) {
            
            _refreshTokenRequestValidator.Validate(request);

            var principal = GetPrincipalFromExpiredToken(request.AccessToken!);

            var id = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if(!Guid.TryParse(id, out var userId)){
                throw new ArgumentException(id, "Invalid Guid");
            }
            var savedRefreshToken = await _tokenRepository.GetRefreshTokenByUserIdAsync(userId);

            if(savedRefreshToken == null) {
                throw new SecurityTokenException("Refresh token not found");
            }
            if(!BCrypt.Net.BCrypt.Verify(request.RefreshToken, savedRefreshToken.TokenHash) || 
             savedRefreshToken.Revoked || savedRefreshToken.ExpiresAt < DateTime.UtcNow) {
                throw new SecurityTokenException("Refresh token expired");
            }

            var newAccessToken = GenerateAccessToken(principal);

            return new RefreshTokenResponse {
                NewAccessToken = newAccessToken
            };
 
        }

        private string GenerateAccessToken(ClaimsPrincipal principal) {

            var signingKey = new SymmetricSecurityKey (Encoding.UTF8.GetBytes(_authSettings.Value.SecretKey!));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var accessToken = new JwtSecurityToken (
                issuer : _authSettings.Value.Issuer,
                audience : _authSettings.Value.Audience,
                claims: principal.Claims,
                expires : DateTime.UtcNow.Add(_authSettings.Value.Expires),
                signingCredentials : credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(accessToken);

        }

        private RefreshTokenDto GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var randomNumGenerator = RandomNumberGenerator.Create();
            randomNumGenerator.GetBytes(randomNumber);
            string rawToken = Convert.ToBase64String(randomNumber);

            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawToken));
            string hashedToken = Convert.ToBase64String(hashBytes);
        
            return new RefreshTokenDto {
                RawToken = rawToken,
                HashToken = hashedToken
            };
        }


        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token) {
            var tokenValidationParameters = new TokenValidationParameters {
                ValidAudience = _authSettings.Value.Audience,
                ValidIssuer = _authSettings.Value.Issuer,
                ValidateIssuer = true,
                ValidateAudience = true,
                IssuerSigningKey =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.Value.SecretKey!)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if(!(securityToken is JwtSecurityToken jwtSecurityToken) ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Refresh token is expired");
            }
            
            return principal;

        }



    }
}
