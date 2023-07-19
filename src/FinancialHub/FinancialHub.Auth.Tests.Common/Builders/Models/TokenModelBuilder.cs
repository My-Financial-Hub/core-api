namespace FinancialHub.Auth.Tests.Common.Builders.Models
{
    public class TokenModelBuilder : AutoFaker<TokenModel>
    {
        public TokenModelBuilder()
        {
            RuleFor(x => x.ExpiresIn, x => x.Date.Soon());
            RuleFor(x => x.Token, x => x.Hashids.Encode(x.Random.Digits(10)));
        }
    }
}
