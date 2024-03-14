using FinancialHub.Core.Application.Extensions;
using FinancialHub.Core.Domain.DTOS.Categories;
using FinancialHub.Core.Domain.DTOS.Transactions;
using FinancialHub.Core.Domain.Interfaces.Resources;
using FinancialHub.Core.Domain.Interfaces.Validators;
using FluentValidation;

namespace FinancialHub.Core.Application.Validators.Transactions
{
    public class TransactionsValidator : ITransactionsValidator
    {
        private const string MESSAGE_LABEL = "Transaction";

        private readonly ITransactionsProvider transactionsProvider;
        private readonly IValidator<CreateTransactionDto> createTransactionDtoValidator;
        private readonly IValidator<UpdateTransactionDto> updateTransactionDtoValidator;

        private readonly IErrorMessageProvider errorMessageProvider;

        public TransactionsValidator(
            ITransactionsProvider transactionsProvider,
            IValidator<CreateTransactionDto> createTransactionDtoValidator, IValidator<UpdateTransactionDto> updateTransactionDtoValidator,
            IErrorMessageProvider errorMessageProvider
        )
        {
            this.transactionsProvider = transactionsProvider;
            this.createTransactionDtoValidator = createTransactionDtoValidator;
            this.updateTransactionDtoValidator = updateTransactionDtoValidator;
            this.errorMessageProvider = errorMessageProvider;
        }

        public async Task<ServiceResult> ExistsAsync(Guid id)
        {
            var result = await this.transactionsProvider.GetByIdAsync(id);
            if (result != null)
            {
                return ServiceResult.Success;
            }

            return new NotFoundError(
                this.errorMessageProvider.NotFoundMessage(MESSAGE_LABEL, id)
            );
        }

        public async Task<ServiceResult> ValidateAsync(CreateTransactionDto createTransactionDto)
        {
            var transactionValid = await this.createTransactionDtoValidator.ValidateAsync(createTransactionDto);
            if (!transactionValid.IsValid)
            {
                return new ValidationError(
                    message: errorMessageProvider.ValidationMessage(MESSAGE_LABEL),
                    errors: transactionValid.Errors.ToFieldValidationError()
                );
            }

            return ServiceResult.Success;
        }

        public async Task<ServiceResult> ValidateAsync(UpdateTransactionDto updateTransactionDto)
        {
            var transactionValid = await this.updateTransactionDtoValidator.ValidateAsync(updateTransactionDto);
            if (!transactionValid.IsValid)
            {
                return new ValidationError(
                    message: errorMessageProvider.ValidationMessage(MESSAGE_LABEL),
                    errors: transactionValid.Errors.ToFieldValidationError()
                );
            }

            return ServiceResult.Success;
        }
    }
}
