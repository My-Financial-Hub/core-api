namespace FinancialHub.Auth.Common.Tests.Builders.Models
{
    public class SignupModelBuilder : AutoFaker<SignupModel>
    {
        public SignupModelBuilder()
        {
            RuleFor(x => x.FirstName, x=> x.Person.FirstName);
            RuleFor(x => x.LastName, x=> x.Person.LastName);
            RuleFor(x => x.Email, x=> x.Person.Email);
            RuleFor(x => x.Password, x=> x.Hashids.Encode(x.Random.Digits(10)));
            RuleFor(x => x.ConfirmPassword, (x, sign)=> sign.Password);
            RuleFor(x => x.BirthDate, x=> x.Person.DateOfBirth);
        }

        public SignupModelBuilder WithFirstName(string firstName)
        {
            RuleFor(x => x.FirstName, firstName);
            return this;
        }

        public SignupModelBuilder WithLastName(string lastName)
        {
            RuleFor(x => x.LastName, lastName);
            return this;
        }

        public SignupModelBuilder WithEmail(string email)
        {
            RuleFor(x => x.Email, email);
            return this;
        }

        public SignupModelBuilder WithValidPassword(string password)
        {
            RuleFor(x => x.Password, password);
            RuleFor(x => x.ConfirmPassword, password);
            return this;
        }

        public SignupModelBuilder WithPassword(string password)
        {
            RuleFor(x => x.Password, password);
            return this;
        }

        public SignupModelBuilder WithConfirmPassword(string password)
        {
            RuleFor(x => x.ConfirmPassword, password);
            return this;
        }

        public SignupModelBuilder WithBirthDate(DateTime birthDate)
        {
            RuleFor(x => x.BirthDate, birthDate);
            return this;
        }
    }
}
