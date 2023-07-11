namespace FinancialHub.Auth.Tests.Common.Builders.Models
{
    public class UserCredentialModelBuilder : AutoFaker<CredentialModel>
    {
        public UserCredentialModelBuilder()
        {
            RuleFor(x => x.Login, X => X.Person.Email);
            RuleFor(x => x.Password, x => x.Hashids.Encode(x.Random.Digits(10)));
        }

        public UserCredentialModelBuilder WithLogin(string? login)
        {
            RuleFor(x => x.Login, login);
            return this;
        }

        public UserCredentialModelBuilder WithPassword(string? password)
        {
            RuleFor(x => x.Password, password);
            return this;
        }

        public UserCredentialModelBuilder WithUserId(Guid? id)
        {
            RuleFor(x => x.UserId, id);
            return this;
        }
    }
}
