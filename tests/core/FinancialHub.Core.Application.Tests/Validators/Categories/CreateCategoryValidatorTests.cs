using FinancialHub.Core.Application.Validators.Categories;
using FinancialHub.Core.Domain.Interfaces.Resources;
using FinancialHub.Core.Domain.Tests.Builders.DTOS.Categories;
using FinancialHub.Core.Resources.Providers;
using System.Globalization;

namespace FinancialHub.Core.Application.Tests.Validators.Categories
{
    public class CreateCategoryValidatorTests
    {
        private CreateCategoryDtoBuilder builder;
        private readonly IValidationErrorMessageProvider errorMessageProvider;
        private readonly CreateCategoryValidator validator;

        public CreateCategoryValidatorTests()
        {
            this.errorMessageProvider = new ValidationErrorMessageProvider(CultureInfo.InvariantCulture);
            this.validator = new CreateCategoryValidator(this.errorMessageProvider);
        }

        [SetUp]
        public void SetUp()
        {
            this.builder = new CreateCategoryDtoBuilder();
        }

        [Test]
        public void CategoryValidator_ValidCategory_ReturnsSuccess()
        {
            var category = builder.Generate();

            var result = validator.Validate(category);

            Assert.IsTrue(result.IsValid);
            Assert.IsEmpty(result.Errors);
        }

        [TestCase("")]
        [TestCase(null)]
        public void CategoryValidator_NullOrEmptyName_ReturnsRequiredError(string invalidName)
        {
            var category = builder.WithName(invalidName).Generate();

            var result = validator.Validate(category);

            Assert.IsFalse(result.IsValid);
            Assert.IsNotEmpty(result.Errors);
            Assert.AreEqual("Name is required", result.Errors[0].ErrorMessage);
        }

        [Test]
        public void CategoryValidator_BigName_ReturnsMaxLengthError()
        {
            var invalidName = new string('a', 201);
            var category = builder.WithName(invalidName).Generate();

            var result = validator.Validate(category);

            Assert.IsFalse(result.IsValid);
            Assert.IsNotEmpty(result.Errors);
            Assert.AreEqual($"Name exceeds the max length of 200", result.Errors[0].ErrorMessage);
        }

        [TestCase("")]
        [TestCase(null)]
        public void CategoryValidator_NullOrEmptyDescription_ReturnsSuccess(string description)
        {
            var category = builder.WithDescription(description).Generate();

            var result = validator.Validate(category);

            Assert.IsTrue(result.IsValid);
            Assert.IsEmpty(result.Errors);
        }

        [Test]
        public void CategoryValidator_BigDescription_ReturnsMaxLengthError()
        {
            var invalidDescription = new string('a', 501);
            var category = builder.WithDescription(invalidDescription).Generate();

            var result = validator.Validate(category);

            Assert.IsFalse(result.IsValid);
            Assert.IsNotEmpty(result.Errors);
            Assert.AreEqual($"Description exceeds the max length of 500", result.Errors[0].ErrorMessage);
        }
    }
}
