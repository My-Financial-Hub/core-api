using FinancialHub.Core.Application.Validators;
using FinancialHub.Core.Domain.Interfaces.Resources;
using FinancialHub.Core.Resources.Providers;
using System.Globalization;

namespace FinancialHub.Core.Application.Tests.Validators
{
    public class AccountValidatorTests
    {
        private AccountModelBuilder builder;
        private readonly AccountValidator validator;
        private readonly IValidationErrorMessageProvider errorMessageProvider;

        public AccountValidatorTests()
        {
            this.errorMessageProvider = new ValidationErrorMessageProvider(CultureInfo.InvariantCulture);
            this.validator = new AccountValidator(this.errorMessageProvider);
        }

        [SetUp]
        public void SetUp()
        {
            this.builder = new AccountModelBuilder();
        }

        [Test]
        public void Account_Valid_ReturnsSuccess()
        {
            var account = builder.Generate();

            var result = validator.Validate(account);

            Assert.IsTrue(result.IsValid);
            Assert.IsEmpty(result.Errors);
        }

        [TestCase("")]
        [TestCase(null)]
        public void Account_NullOrEmptyName_ReturnsRequiredError(string invalidName)
        {
            var account = builder.WithName(invalidName).Generate();

            var result = validator.Validate(account);

            Assert.IsFalse(result.IsValid);
            Assert.IsNotEmpty(result.Errors);
            Assert.AreEqual("Name is required",result.Errors[0].ErrorMessage);
        }

        [Test]
        public void Account_BigName_ReturnsMaxLengthError()
        {
            var invalidName = new string('a',201);
            var account = builder.WithName(invalidName).Generate();

            var result = validator.Validate(account);

            Assert.IsFalse(result.IsValid);
            Assert.IsNotEmpty(result.Errors);
            Assert.AreEqual("Name exceeds the max length of 200", result.Errors[0].ErrorMessage);
        }

        [TestCase("")]
        [TestCase(null)]
        public void Account_NullOrEmptyDescription_ReturnsSuccess(string description)
        {
            var account = builder.WithDescription(description).Generate();

            var result = validator.Validate(account);

            Assert.IsTrue(result.IsValid);
            Assert.IsEmpty(result.Errors);
        }

        [Test]
        public void Account_BigDescription_ReturnsMaxLengthError()
        {
            var invalidDescription = new string('a', 501);
            var account = builder.WithDescription(invalidDescription).Generate();

            var result = validator.Validate(account);

            Assert.IsFalse(result.IsValid);
            Assert.IsNotEmpty(result.Errors);
            Assert.AreEqual("Description exceeds the max length of 500", result.Errors[0].ErrorMessage);
        }
    }
}
