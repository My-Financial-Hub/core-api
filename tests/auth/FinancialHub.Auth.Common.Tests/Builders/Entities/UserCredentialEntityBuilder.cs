namespace FinancialHub.Auth.Common.Tests.Builders.Entities
{
    public class UserCredentialEntityBuilder : BaseEntityBuilder<CredentialEntity>
    {
        private readonly UserEntityBuilder userBuilder;
        public UserCredentialEntityBuilder()
        {
            this.userBuilder = new UserEntityBuilder();

            var user = userBuilder.Generate();
            RuleFor(x => x.Login, x => x.Person.Email);
            RuleFor(x => x.Password, x => x.Hashids.Encode(x.Random.Digits(10)));
            RuleFor(x => x.User, user);
            RuleFor(x => x.UserId, user.Id);
        }

        public UserCredentialEntityBuilder WithLogin(string? login)
        {
            RuleFor(x => x.Login, login);
            return this;
        }

        public UserCredentialEntityBuilder WithPassword(string? password)
        {
            RuleFor(x => x.Password, password);
            return this;
        }

        public UserCredentialEntityBuilder WithUserId(Guid? id)
        {
            RuleFor(x => x.UserId, id);
            this.WithUser(default);
            return this;
        }

        public UserCredentialEntityBuilder WithUser(UserEntity? user)
        {
            RuleFor(x => x.User, user);
            return this;
        }
    }
}
