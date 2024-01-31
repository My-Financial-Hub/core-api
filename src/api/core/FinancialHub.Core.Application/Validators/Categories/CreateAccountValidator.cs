using FinancialHub.Core.Domain.DTOS.Categories;
using FinancialHub.Core.Domain.Interfaces.Resources;
using FluentValidation;

namespace FinancialHub.Core.Application.Validators.Categories
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryValidator(IValidationErrorMessageProvider errorMessageProvider)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(errorMessageProvider.Required)
                .Length(0, 200)
                .WithMessage(errorMessageProvider.ExceedMaxLength);

            RuleFor(x => x.Description)
                .Length(0, 500)
                .WithMessage(errorMessageProvider.ExceedMaxLength);
        }
    }
}
