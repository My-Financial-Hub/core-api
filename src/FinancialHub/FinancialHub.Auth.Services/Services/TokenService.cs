using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using FinancialHub.Auth.Domain.Interfaces.Services;
using FinancialHub.Auth.Domain.Models;
using FinancialHub.Auth.Services.Configurations;
using System.Security.Claims;

namespace FinancialHub.Auth.Services.Services
{
    public class TokenService : ITokenService
    {
        private readonly TokenServiceSettings settings;
        public TokenService(IOptions<TokenServiceSettings> settings)
        {
            this.settings = settings.Value;
        }

        private SigningCredentials Credentials 
        {
            get
            {
                var key = Encoding.ASCII.GetBytes(this.settings.SecurityKey);
                var securityKey = new SymmetricSecurityKey(key);
                return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            }
        }

        private static ClaimsIdentity GenerateUserClaims(UserModel user)
        {
            return new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()!)
                }
            );
        }

        public TokenModel GenerateToken(UserModel user)
        {
            var expires = DateTime.UtcNow.AddMinutes(60);
            var handler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Expires = expires,
                SigningCredentials = this.Credentials,
                Subject = GenerateUserClaims(user)
            };

            var securityToken = handler.CreateToken(tokenDescriptor);
            var token = handler.WriteToken(securityToken);

            return new TokenModel(token, expires);
        }
    }
}
