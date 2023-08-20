using FinancialHub.Core.Domain.Enums;
using FinancialHub.Core.Services.Validators;

namespace FinancialHub.Core.Services.NUnitTests.Validators
{
    public class TransactionValidatorTests
    {
        private TransactionModelBuilder builder;
        private readonly TransactionValidator validator;

        public TransactionValidatorTests()
        {
            this.validator = new TransactionValidator();
        }

        [SetUp]
        public void SetUp()
        {
            this.builder = new TransactionModelBuilder();
        }

        [Test]
        public void TransactionValidator_ValidCategory_ReturnsSuccess()
        {
            var transaction = builder.Generate();

            var result = validator.Validate(transaction);

            Assert.IsTrue(result.IsValid);
            Assert.IsEmpty(result.Errors);
        }

        [TestCase("")]
        [TestCase(null)]
        public void TransactionValidator_NullOrEmptyDescription_ReturnsSuccess(string description)
        {
            var transaction = builder.WithDescription(description).Generate();

            var result = validator.Validate(transaction);

            Assert.IsTrue(result.IsValid);
            Assert.IsEmpty(result.Errors);
        }

        [Test]
        public void TransactionValidator_BigDescription_ReturnsMaxLengthError()
        {
            var invalidDescription = new string('a', 501);
            var transaction = builder.WithDescription(invalidDescription).Generate();

            var result = validator.Validate(transaction);

            Assert.IsFalse(result.IsValid);
            Assert.IsNotEmpty(result.Errors);
            Assert.AreEqual("Description exceeds the max length of 500", result.Errors[0].ErrorMessage);
        }

        [Test]
        public void TransactionValidator_TypeOutOfEnum_ReturnsError()
        {
            var type = 999;
            var transaction = builder.WithType((TransactionType)type).Generate();

            var result = validator.Validate(transaction);

            Assert.IsFalse(result.IsValid);
            Assert.IsNotEmpty(result.Errors);
            Assert.AreEqual("Type has a range of values which does not include '999'", result.Errors[0].ErrorMessage);
        }

        [Test]
        public void TransactionValidator_StatusOutOfEnum_ReturnsError()
        {
            var status = 999;
            var transaction = builder.WithStatus((TransactionStatus)status).Generate();

            var result = validator.Validate(transaction);

            Assert.IsFalse(result.IsValid);
            Assert.IsNotEmpty(result.Errors);
            Assert.AreEqual("Status has a range of values which does not include '999'", result.Errors[0].ErrorMessage);
        }
    }
}
