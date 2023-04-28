using FinancialHub.Auth.Services.Validators;
using FinancialHub.Auth.Tests.Common.Builders.Entities;

namespace FinancialHub.Auth.Services.Tests.Validators
{
    public class UserValidatorTests
    {
        private UserValidator validator;
        private UserModelBuilder builder;

        [SetUp]
        public void SetUp()
        {
            validator = new UserValidator();
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

            var result = validator.Validate(user);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo("Email is required"));
            });
        }

        [Test]
        public void Validate_InvalidEmail_ReturnsFalse()
        {
            var user = builder
                .WithEmail(new string('a', 3))
                .Generate();

            var result = validator.Validate(user);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo("Email is invalid"));
            });
        }

        [Test]
        public void Validate_EmailOverMaxLength_ReturnsFalse()
        {
            var user = builder
                .WithEmail(new string('a', 301) + "@test.com")
                .Generate();

            var result = validator.Validate(user);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo("Email exceeds the max length of 300"));
            });
        }

        [Test]
        public void Validate_FirstNameEmpty_ReturnsFalse()
        {
            var user = builder
                .WithName(string.Empty)
                .Generate();

            var result = validator.Validate(user);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo("FirstName is required"));
            });
        }

        [Test]
        public void Validate_FirstNameOverMaxLength_ReturnsFalse()
        {
            var user = builder
                .WithName(new string('a', 301))
                .Generate();

            var result = validator.Validate(user);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo("FirstName exceeds the max length of 300"));
            });
        }

        [Test]
        public void Validate_LastNameEmpty_ReturnsFalse()
        {
            var user = builder
                .WithLastName(string.Empty)
                .Generate();

            var result = validator.Validate(user);

            Assert.That(result.IsValid, Is.False);
        }

        [Test]
        public void Validate_LastNameOverMaxLength_ReturnsFalse()
        {
            var user = builder
                .WithLastName(new string('a', 301))
                .Generate();

            var result = validator.Validate(user);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo("LastName exceeds the max length of 300"));
            });
        }
    }
}
