using Bogus;
using FinancialHub.Auth.Domain.Models;

namespace FinancialHub.Auth.Tests.Commom.Builders.Entities
{
    public class UserModelBuilder : Faker<UserModel>
    {
        public UserModelBuilder()
        {
            this.RuleFor(x => x.FirstName, x => x.Person.FirstName);
            this.RuleFor(x => x.LastName, x => x.Person.LastName);
            this.RuleFor(x => x.BirthDate, x => x.Person.DateOfBirth);
            this.RuleFor(x => x.Email, x => x.Person.Email);
            this.RuleFor(x => x.Password, x => x.Random.AlphaNumeric(12));
        }

        public UserModelBuilder WithName(string name)
        {
            this.RuleFor(x => x.FirstName, name);
            return this;
        }

        public UserModelBuilder WithLastName(string name)
        {
            this.RuleFor(x => x.LastName, name);
            return this;
        }

        public UserModelBuilder WithBirthDate(DateTime date)
        {
            this.RuleFor(x => x.BirthDate, date);
            return this;
        }

        public UserModelBuilder WithEmail(string email)
        {
            this.RuleFor(x => x.Email, email);
            return this;
        }

        public UserModelBuilder WithPassword(string password)
        {
            this.RuleFor(x => x.Password, password);
            return this;
        }
    }
}
