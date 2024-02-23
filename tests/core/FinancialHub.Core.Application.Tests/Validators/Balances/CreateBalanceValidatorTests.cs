using FinancialHub.Core.Application.Validators.Balances;
using FinancialHub.Core.Domain.Interfaces.Resources;
using FinancialHub.Core.Domain.Tests.Builders.DTOS.Balances;
using FinancialHub.Core.Resources.Providers;
using System.Globalization;

namespace FinancialHub.Core.Application.Tests.Validators.Balances
{
    public class CreateBalanceValidatorTests
    {
        private CreateBalanceDtoBuilder builder;
        private readonly CreateBalanceValidator validator;
        private readonly IValidationErrorMessageProvider errorMessageProvider;

        public CreateBalanceValidatorTests()
        {
            this.errorMessageProvider = new ValidationErrorMessageProvider(CultureInfo.InvariantCulture);
            this.validator = new CreateBalanceValidator(this.errorMessageProvider);
        }

        [SetUp]
        public void SetUp()
        {
            this.builder = new CreateBalanceDtoBuilder();
        }

        [Test]
        public void Balance_Valid_ReturnsSuccess()
        {
            var balance = builder.Generate();

            var result = validator.Validate(balance);

            Assert.IsTrue(result.IsValid);
            Assert.IsEmpty(result.Errors);
        }

        [TestCase("")]
        [TestCase(null)]
        public void Balance_NullOrEmptyName_ReturnsRequiredError(string invalidName)
        {
            var account = builder.WithName(invalidName).Generate();

            var result = validator.Validate(account);

            Assert.IsFalse(result.IsValid);
            Assert.IsNotEmpty(result.Errors);
            Assert.AreEqual("Name is required", result.Errors[0].ErrorMessage);
        }

        [Test]
        public void Balance_BigName_ReturnsMaxLengthError()
        {
            var invalidName = new string('a', 201);
            var account = builder.WithName(invalidName).Generate();

            var result = validator.Validate(account);

            Assert.IsFalse(result.IsValid);
            Assert.IsNotEmpty(result.Errors);
            Assert.AreEqual($"Name exceeds the max length of 200", result.Errors[0].ErrorMessage);
        }

        [Test]
        public void Balance_EmptyAccountId_ReturnsRequiredError()
        {
            var invalidId = Guid.Empty;
            var account = builder.WithAccountId(invalidId).Generate();

            var result = validator.Validate(account);

            Assert.IsFalse(result.IsValid);
            Assert.IsNotEmpty(result.Errors);
            Assert.AreEqual("Account Id is required", result.Errors[0].ErrorMessage);
        }

        [Test]
        public void Balance_NullAccountId_ReturnsRequiredError()
        {
            var account = builder.WithAccountId((Guid)default).Generate();

            var result = validator.Validate(account);

            Assert.IsFalse(result.IsValid);
            Assert.IsNotEmpty(result.Errors);
            Assert.AreEqual("Account Id is required", result.Errors[0].ErrorMessage);
        }
    }
}
