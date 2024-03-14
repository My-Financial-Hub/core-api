using FinancialHub.Core.Application.Extensions;
using FinancialHub.Core.Domain.DTOS.Categories;
using FinancialHub.Core.Domain.Interfaces.Resources;
using FinancialHub.Core.Domain.Interfaces.Validators;
using FluentValidation;

namespace FinancialHub.Core.Application.Validators.Categories
{
    public class CategoriesValidator : ICategoriesValidator
    {
        private const string MESSAGE_LABEL = "Category";

        private readonly ICategoriesProvider categoriesProvider;
        private readonly IValidator<CreateCategoryDto> createCategoryDtoValidator;
        private readonly IValidator<UpdateCategoryDto> updateCategoryDtoValidator;

        private readonly IErrorMessageProvider errorMessageProvider;

        public CategoriesValidator(
            ICategoriesProvider categoriesProvider,
            IValidator<CreateCategoryDto> createCategoryDtoValidator, IValidator<UpdateCategoryDto> updateCategoryDtoValidator ,
            IErrorMessageProvider errorMessageProvider
        )
        {
            this.categoriesProvider = categoriesProvider;
            this.createCategoryDtoValidator = createCategoryDtoValidator;
            this.updateCategoryDtoValidator = updateCategoryDtoValidator;
            this.errorMessageProvider = errorMessageProvider;
        }

        public async Task<ServiceResult> ExistsAsync(Guid id)
        {
            var result = await this.categoriesProvider.GetByIdAsync(id);
            if (result != null)
            {
                return ServiceResult.Success;
            }

            return new NotFoundError(
                this.errorMessageProvider.NotFoundMessage(MESSAGE_LABEL, id)
            );
        }

        public async Task<ServiceResult> ValidateAsync(CreateCategoryDto createCategoryDto)
        {
            var balanceValid = await this.createCategoryDtoValidator.ValidateAsync(createCategoryDto);
            if (!balanceValid.IsValid)
            {
                return new ValidationError(
                    message: errorMessageProvider.ValidationMessage(MESSAGE_LABEL),
                    errors: balanceValid.Errors.ToFieldValidationError()
                );
            }

            return ServiceResult.Success;
        }

        public async Task<ServiceResult> ValidateAsync(UpdateCategoryDto updateCategoryDto)
        {
            var categoryValid = await this.updateCategoryDtoValidator.ValidateAsync(updateCategoryDto);
            if (!categoryValid.IsValid)
            {
                return new ValidationError(
                    message: errorMessageProvider.ValidationMessage(MESSAGE_LABEL),
                    errors: categoryValid.Errors.ToFieldValidationError()
                );
            }

            return ServiceResult.Success;
        }
    }
}
