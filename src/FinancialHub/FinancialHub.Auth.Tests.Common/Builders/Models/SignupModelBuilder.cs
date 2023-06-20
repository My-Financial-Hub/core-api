using AutoBogus;

namespace FinancialHub.Auth.Tests.Common.Builders.Models
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
    }
}
