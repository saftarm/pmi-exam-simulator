using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestAPI.Entities;
using TestAPI.Services.Interfaces;
namespace TestAPI.Services.Implementation


{
    public class JWTService : IJWTService
    {
        private readonly IOptions<AuthSettings> _authSettings;

        public JWTService(IOptions<AuthSettings> authSettings)
        {

            _authSettings = authSettings;
        }
        public string GenerateToken(User user)
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

            // Implementation for generating JWT token based on the account details

            var jwtToken = new JwtSecurityToken(

                issuer: _authSettings.Value.Issuer,
                audience: _authSettings.Value.Audience,
                expires: DateTime.UtcNow.Add(_authSettings.Value.Expires),
                claims: claims,
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }




    }
}
