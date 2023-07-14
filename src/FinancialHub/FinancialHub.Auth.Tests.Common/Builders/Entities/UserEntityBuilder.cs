namespace FinancialHub.Auth.Tests.Common.Builders.Entities
{
    public class UserEntityBuilder : BaseEntityBuilder<UserEntity>
    {
        public UserEntityBuilder()
        {
            this.RuleFor(x => x.FirstName, x => x.Person.FirstName);
            this.RuleFor(x => x.LastName, x => x.Person.LastName);
            this.RuleFor(x => x.BirthDate, x => x.Person.DateOfBirth);
            this.RuleFor(x => x.Email, x => x.Person.Email);
        }

        public UserEntityBuilder WithName(string name)
        {
            this.RuleFor(x => x.FirstName, name);
            return this;
        }

        public UserEntityBuilder WithLastName(string name)
        {
            this.RuleFor(x => x.LastName, name);
            return this;
        }

        public UserEntityBuilder WithBirthDate(DateTime date)
        {
            this.RuleFor(x => x.BirthDate, date);
            return this;
        }

        public UserEntityBuilder WithEmail(string email)
        {
            this.RuleFor(x => x.Email, email);
            return this;
        }
    }
}
