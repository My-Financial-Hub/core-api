using FinancialHub.Auth.Domain.Interfaces.Resources;
using FinancialHub.Auth.Resources.Providers;
using FinancialHub.Auth.Services.Validators;
using FinancialHub.Auth.Tests.Common.Builders.Models;
using System.Globalization;

namespace FinancialHub.Auth.Services.Tests.Validators
{
    public class UserValidatorTests
    {
        private IErrorMessageProvider errorMessageProvider;

        private UserValidator validator;
        private UserModelBuilder builder;

        [SetUp]
        public void SetUp()
        {
            errorMessageProvider = new ErrorMessageProvider(CultureInfo.InvariantCulture);
            validator = new UserValidator(errorMessageProvider);
            builder = new UserModelBuilder();
        }

        [Test]
        public void Validate_ValidUser_ReturnsTrue()
        {
            var user = builder.Generate();

            var result = validator.Validate(user);

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

        [Test]
        public void Validate_EmailOverMaxLength_ReturnsFalse()
        {
            var user = builder
                .WithEmail(new string('a', 301) + "@test.com")
                .Generate();

            var expectedMessage = "Email exceeds the max length of 300";

            var result = validator.Validate(user);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo(expectedMessage));
            });
        }

        [Test]
        public void Validate_FirstNameEmpty_ReturnsFalse()
        {
            var user = builder
                .WithName(string.Empty)
                .Generate();

            var expectedMessage = "First Name is required";

            var result = validator.Validate(user);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo(expectedMessage));
            });
        }

        [Test]
        public void Validate_FirstNameOverMaxLength_ReturnsFalse()
        {
            var user = builder
                .WithName(new string('a', 301))
                .Generate();

            var expectedMessage = "First Name exceeds the max length of 300";

            var result = validator.Validate(user);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo(expectedMessage));
            });
        }

        [Test]
        public void Validate_LastNameEmpty_ReturnsFalse()
        {
            var user = builder
                .WithLastName(string.Empty)
                .Generate();

            var expectedMessage = "Last Name is required";

            var result = validator.Validate(user);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo(expectedMessage));
            });
        }

        [Test]
        public void Validate_LastNameOverMaxLength_ReturnsFalse()
        {
            var user = builder
                .WithLastName(new string('a', 301))
                .Generate();

            var expectedMessage = "Last Name exceeds the max length of 300";

            var result = validator.Validate(user);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo(expectedMessage));
            });
        }

        [Test]
        public void Validate_BirthDateEmpty_ReturnsFalse()
        {
            var user = builder
                .WithBirthDate(default)
                .Generate();

            var expectedMessage = "Birth Date is required";

            var result = validator.Validate(user);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo(expectedMessage));
            });
        }
    }
}
