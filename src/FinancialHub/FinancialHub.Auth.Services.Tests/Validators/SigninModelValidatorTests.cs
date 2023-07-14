using FinancialHub.Auth.Domain.Interfaces.Resources;
using FinancialHub.Auth.Resources.Providers;
using FinancialHub.Auth.Services.Validators;
using System.Globalization;

namespace FinancialHub.Auth.Services.Tests.Validators
{
    public class SigninModelValidatorTests
    {
        private IErrorMessageProvider errorMessageProvider;
        private SigninModelValidator validator;
        private SigninModelBuilder builder;

        [SetUp]
        public void SetUp()
        {
            errorMessageProvider = new ErrorMessageProvider(CultureInfo.InvariantCulture);
            validator = new SigninModelValidator(errorMessageProvider);
            builder = new SigninModelBuilder();
        }

        [Test]
        public void Validate_ValidSignupModel_ReturnsTrue()
        {
            var signup = builder.Generate();

            var result = validator.Validate(signup);

            Assert.That(result.IsValid, Is.True);
        }

        [Test]
        public void Validate_EmailEmpty_ReturnsFalse()
        {
            var user = builder
                .WithEmail(string.Empty)
                .Generate();

            var expectedMessage = "Email is required";

            var result = validator.Validate(user);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo(expectedMessage));
            });
        }

        [Test]
        public void Validate_InvalidEmail_ReturnsFalse()
        {
            var user = builder
                .WithEmail(new string('a', 3))
                .Generate();

            var expectedMessage = "Email is invalid";

            var result = validator.Validate(user);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo(expectedMessage));
            });
        }


        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        [TestCase(7)]
        public void Validate_PasswordLessThan_ReturnsFalse(int size)
        {
            var user = builder
                .WithPassword(new string('a', size))
                .Generate();

            var expectedMessage = "Password needs to have at least the lenght of 8";

            var result = validator.Validate(user);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo(expectedMessage));
            });
        }

        [Test]
        public void Validate_PasswordOverMaxLength_ReturnsFalse()
        {
            var user = builder
                .WithPassword(new string('a', 301))
                .Generate();

            var expectedMessage = "Password exceeds the max length of 80";

            var result = validator.Validate(user);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo(expectedMessage));
            });
        }

        [TestCase(8)]
        [TestCase(10)]
        [TestCase(50)]
        [TestCase(80)]
        public void Validate_ValidPassword_ReturnsTrue(int size)
        {
            var user = builder
                .WithPassword(new string('a', size))
                .Generate();

            var result = validator.Validate(user);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsValid, Is.True);
                Assert.That(result.Errors, Is.Empty);
            });
        }
    }
}
