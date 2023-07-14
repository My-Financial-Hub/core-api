namespace FinancialHub.Auth.Tests.Common.Builders.Models
{
    public class SigninModelBuilder : AutoFaker<SigninModel>
    {
        public SigninModelBuilder()
        {
            RuleFor(x => x.Email, x => x.Person.Email);
            RuleFor(x => x.Password, x=> x.Hashids.Encode(x.Random.Digits(10)));
        }

        public SigninModelBuilder WithEmail(string email)
        {
            RuleFor(x => x.Email, email);
            return this;
        }

        public SigninModelBuilder WithPassword(string password)
        {
            RuleFor(x => x.Password, password);
            return this;
        }
    }
}
