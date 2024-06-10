using FinancialHub.Core.Application.Extensions;
using FinancialHub.Core.Domain.DTOS.Categories;
using FinancialHub.Core.Domain.Interfaces.Resources;
using FinancialHub.Core.Domain.Interfaces.Validators;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace FinancialHub.Core.Application.Validators.Categories
{
    internal class CategoriesValidator : ICategoriesValidator
    {
        private const string MESSAGE_LABEL = "Category";

        private readonly ICategoriesProvider categoriesProvider;
        private readonly IValidator<CreateCategoryDto> createCategoryDtoValidator;
        private readonly IValidator<UpdateCategoryDto> updateCategoryDtoValidator;

        private readonly IErrorMessageProvider errorMessageProvider;
        private readonly ILogger<CategoriesValidator> logger;

        public CategoriesValidator(
            ICategoriesProvider categoriesProvider,
            IValidator<CreateCategoryDto> createCategoryDtoValidator, IValidator<UpdateCategoryDto> updateCategoryDtoValidator,
            IErrorMessageProvider errorMessageProvider, ILogger<CategoriesValidator> logger
        )
        {
            this.categoriesProvider = categoriesProvider;
            this.createCategoryDtoValidator = createCategoryDtoValidator;
            this.updateCategoryDtoValidator = updateCategoryDtoValidator;
            this.errorMessageProvider = errorMessageProvider;
            this.logger = logger;
        }

        public async Task<ServiceResult> ExistsAsync(Guid id)
        {
            this.logger.LogInformation("Validating the existence of category {id}", id);
            var result = await this.categoriesProvider.GetByIdAsync(id);
            if (result != null)
            {
                this.logger.LogInformation("Category with {id} exists", id);
                return ServiceResult.Success;
            }

            var errorMessage = this.errorMessageProvider.NotFoundMessage(MESSAGE_LABEL, id);
            this.logger.LogWarning("Validation error : {message}", errorMessage);
            return new NotFoundError(errorMessage);
        }

        public async Task<ServiceResult> ValidateAsync(CreateCategoryDto createCategoryDto)
        {
            this.logger.LogInformation("Validating Create Category data");
            var result = await this.createCategoryDtoValidator.ValidateAsync(createCategoryDto);
            if (!result.IsValid)
            {
                var errorMessage = errorMessageProvider.ValidationMessage(MESSAGE_LABEL);
                this.logger.LogWarning("Validation error : {message}", errorMessage);

                var fieldErrors = result.Errors.ToFieldValidationError();
                this.logger.LogTrace("Validation field errors : {fieldErrors}", fieldErrors);

                return new ValidationError(message: errorMessage, errors: fieldErrors);
            }

            this.logger.LogInformation("Create Category data validated");
            return ServiceResult.Success;
        }

        public async Task<ServiceResult> ValidateAsync(UpdateCategoryDto updateCategoryDto)
        {
            this.logger.LogInformation("Validating Update Category data");
            var result = await this.updateCategoryDtoValidator.ValidateAsync(updateCategoryDto);
            if (!result.IsValid)
            {
                var errorMessage = errorMessageProvider.ValidationMessage(MESSAGE_LABEL);
                this.logger.LogWarning("Validation error : {message}", errorMessage);

                var fieldErrors = result.Errors.ToFieldValidationError();
                this.logger.LogTrace("Validation field errors : {fieldErrors}", fieldErrors);

                return new ValidationError(message: errorMessage, errors: fieldErrors);
            }

            this.logger.LogInformation("Create Update data validated");
            return ServiceResult.Success;
        }
    }
}
