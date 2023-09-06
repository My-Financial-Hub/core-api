using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace FinancialHub.Auth.Application.Tests.Services
{
    public partial class TokenServiceTests
    {
        [Test]
        public void GenerateToken_ValidUser_ReturnsValidToken()
        {
            var userModel = userModelBuilder.Generate();

            var tokenModel = service.GenerateToken(userModel);

            Assert.That(new JwtSecurityTokenHandler().CanReadToken(tokenModel.Token), Is.True);
        }

        [Test]
        public void GenerateToken_ValidUser_ReturnsTokenWithValidExpirationDate()
        {
            var expectedExpirationDate = DefaultOption.Expires;
            var minimumExpected = DateTime.UtcNow.AddMinutes(expectedExpirationDate - 1);
            var maximumExpected = DateTime.UtcNow.AddMinutes(expectedExpirationDate + 1);
            var userModel = userModelBuilder.Generate();

            var tokenModel = service.GenerateToken(userModel);

            var token = new JwtSecurityTokenHandler().ReadJwtToken(tokenModel.Token);
            Assert.That(token.ValidTo, Is.InRange(minimumExpected, maximumExpected));
        }

        [Test]
        public void GenerateToken_ValidUser_ReturnsTokenWithUserData()
        {
            var userModel = userModelBuilder.Generate();

            var tokenModel = service.GenerateToken(userModel);

            var token = new JwtSecurityTokenHandler().ReadJwtToken(tokenModel.Token);
            Assert.Multiple(() =>
            {
                Assert.That(token.Payload.Jti,      Is.EqualTo(userModel.Id.ToString()));
                Assert.That(token.Payload["email"], Is.EqualTo(userModel.Email));
                Assert.That(token.Payload["name"],  Is.EqualTo(userModel.FirstName));
            });
        }

        [Test]
        public void GenerateToken_ValidUser_ReturnsTokenWithValidSignature()
        {
            var userModel = userModelBuilder.Generate();

            var tokenModel = service.GenerateToken(userModel);

            var key = Encoding.ASCII.GetBytes(DefaultOption.SecurityKey);
            var handler = new JwtSecurityTokenHandler();
            handler.ValidateToken(
                tokenModel.Token, 
                new TokenValidationParameters() 
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                },
                out var token
            );

            Assert.Multiple(() =>
            {
                var signinKey = (SymmetricSecurityKey)token.SigningKey;
                Assert.That(signinKey.IsSupportedAlgorithm(SecurityAlgorithms.HmacSha256Signature));
                Assert.That(signinKey.Key, Is.EqualTo(key));
            });
        }
    }
}
