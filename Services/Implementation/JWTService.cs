using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TestAPI.Data;
using TestAPI.DTO;
using TestAPI.Entities;
using TestAPI.Models;
using TestAPI.Persistence.Interfaces;
using TestAPI.ResultPattern;
using TestAPI.Services.Interfaces;
using TestAPI.Validation;
using Microsoft.EntityFrameworkCore;

namespace TestAPI.Services.Implementation
{
    public class JWTService : IJWTService
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenRepository _tokenRepository;
        private readonly IOptions<AuthSettings> _authSettings;
        private readonly IValidatorResolver _validatorResolver;

        public JWTService(
            IOptions<AuthSettings> authSettings,
            ApplicationDbContext context,
            ITokenRepository tokenRepository,
            IValidatorResolver validatorResolver)
        {
            _authSettings = authSettings;
            _context = context;
            _tokenRepository = tokenRepository;
            _validatorResolver = validatorResolver;
        }

        public async Task<Result<TokenResponse>> ProvideTokens(User user)
        {
            var secretKey = _authSettings.Value.SecretKey;
            if (string.IsNullOrWhiteSpace(secretKey))
            {
                return Result<TokenResponse>.Failure(Errors.InvalidToken);
            }

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName!),
                new(ClaimTypes.Role, user.Role.ToString()),
            };

            var jwtToken = new JwtSecurityToken(
                issuer: _authSettings.Value.Issuer,
                audience: _authSettings.Value.Audience,
                expires: DateTime.UtcNow.Add(_authSettings.Value.Expires),
                claims: claims,
                signingCredentials: credentials);

            var refreshTokens = GenerateRefreshToken();

            var tokens = new TokenResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                RefreshToken = refreshTokens.RawToken,
            };

            var existingRefreshToken = await _context.RefreshTokens
                .OrderByDescending(t => t.CreatedAt)
                .FirstOrDefaultAsync(t => t.UserId == user.Id);

            if (existingRefreshToken != null)
            {
                existingRefreshToken.Revoked = true;
                await _context.SaveChangesAsync();
            }

            var newRefreshToken = new RefreshToken
            {
                UserId = user.Id,
                TokenHash = refreshTokens.HashToken,
                ExpiresAt = DateTime.UtcNow.AddDays(14),
                Revoked = false,
            };

            await _context.RefreshTokens.AddAsync(newRefreshToken);
            await _context.SaveChangesAsync();

            return Result<TokenResponse>.Success(tokens);
        }

        public async Task<Result<RefreshTokenResponse>> RefreshToken(RefreshTokenRequest request)
        {
            var validationResult = await _validatorResolver.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Result<RefreshTokenResponse>.Failure(Errors.ValidationFailed);
            }

            ClaimsPrincipal principal;
            try
            {
                principal = GetPrincipalFromExpiredToken(request.AccessToken!);
            }
            catch (SecurityTokenException)
            {
                return Result<RefreshTokenResponse>.Failure(Errors.InvalidToken);
            }

            var id = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(id, out var userId))
            {
                return Result<RefreshTokenResponse>.Failure(Errors.InvalidToken);
            }

            var savedRefreshToken = await _tokenRepository.GetRefreshTokenByUserIdAsync(userId);
            if (savedRefreshToken == null)
            {
                return Result<RefreshTokenResponse>.Failure(Errors.InvalidToken);
            }

            if (!VerifyRefreshTokenHash(request.RefreshToken!, savedRefreshToken.TokenHash)
                || savedRefreshToken.Revoked
                || savedRefreshToken.ExpiresAt < DateTime.UtcNow)
            {
                return Result<RefreshTokenResponse>.Failure(Errors.InvalidToken);
            }

            var newAccessToken = GenerateAccessToken(principal);

            return Result<RefreshTokenResponse>.Success(new RefreshTokenResponse
            {
                NewAccessToken = newAccessToken,
            });
        }

        private string GenerateAccessToken(ClaimsPrincipal principal)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.Value.SecretKey!));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var accessToken = new JwtSecurityToken(
                issuer: _authSettings.Value.Issuer,
                audience: _authSettings.Value.Audience,
                claims: principal.Claims,
                expires: DateTime.UtcNow.Add(_authSettings.Value.Expires),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(accessToken);
        }

        private static string HashRefreshToken(string rawToken)
        {
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawToken));
            return Convert.ToBase64String(hashBytes);
        }

        private static bool VerifyRefreshTokenHash(string rawToken, string storedHash)
        {
            var computed = HashRefreshToken(rawToken);
            return CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(computed),
                Encoding.UTF8.GetBytes(storedHash));
        }

        private RefreshTokenDto GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var randomNumGenerator = RandomNumberGenerator.Create();
            randomNumGenerator.GetBytes(randomNumber);
            string rawToken = Convert.ToBase64String(randomNumber);
            string hashedToken = HashRefreshToken(rawToken);

            return new RefreshTokenDto
            {
                RawToken = rawToken,
                HashToken = hashedToken,
            };
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidAudience = _authSettings.Value.Audience,
                ValidIssuer = _authSettings.Value.Issuer,
                ValidateIssuer = true,
                ValidateAudience = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.Value.SecretKey!)),
                ValidateLifetime = false,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken
                || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}
