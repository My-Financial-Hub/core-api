using FinancialHub.Core.Domain.DTOS.Accounts;
using FinancialHub.Core.Domain.Interfaces.Validators;
using FluentValidation;
using static FinancialHub.Common.Results.Errors.ValidationError;

namespace FinancialHub.Core.Application.Validators.Accounts
{
    public class AccountValidator : IAccountValidator
    {
        private readonly IValidator<CreateAccountDto> createValidator;
        private readonly IValidator<UpdateAccountDto> updateAccountDto;

        public AccountValidator(
            IValidator<CreateAccountDto> createValidator,
            IValidator<UpdateAccountDto> updateAccountDto
        )
        {
            this.createValidator = createValidator;
            this.updateAccountDto = updateAccountDto;
        }

        public async Task<ServiceResult> ValidateAsync(CreateAccountDto createAccountDto)
        {
            var result = await this.createValidator.ValidateAsync(createAccountDto);

            if (result.IsValid)
            {
                return ServiceResult.Success;
            }

            var errors = result.Errors
                .GroupBy(x => x.PropertyName)
                .Select(x => 
                    new FieldValidationError(
                        field: x.Key,
                        messages: x.Select(y => y.ErrorMessage).ToArray()
                    )
                 ).ToArray();
            
            return new ValidationError("Account validation error", errors);
        }

        public async Task<ServiceResult> ValidateAsync(UpdateAccountDto updateAccountDto)
        {
            throw new NotImplementedException();
        }
    }
}
