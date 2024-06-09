using FinancialHub.Core.Application.Extensions;
using FinancialHub.Core.Domain.DTOS.Categories;
using FinancialHub.Core.Domain.DTOS.Transactions;
using FinancialHub.Core.Domain.Interfaces.Resources;
using FinancialHub.Core.Domain.Interfaces.Validators;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace FinancialHub.Core.Application.Validators.Transactions
{
    internal class TransactionsValidator : ITransactionsValidator
    {
        private const string MESSAGE_LABEL = "Transaction";

        private readonly ITransactionsProvider transactionsProvider;
        private readonly IValidator<CreateTransactionDto> createTransactionDtoValidator;
        private readonly IValidator<UpdateTransactionDto> updateTransactionDtoValidator;

        private readonly IErrorMessageProvider errorMessageProvider;
        private readonly ILogger<TransactionsValidator> logger;

        public TransactionsValidator(
            ITransactionsProvider transactionsProvider,
            IValidator<CreateTransactionDto> createTransactionDtoValidator, IValidator<UpdateTransactionDto> updateTransactionDtoValidator,
            IErrorMessageProvider errorMessageProvider, ILogger<TransactionsValidator> logger
        )
        {
            this.transactionsProvider = transactionsProvider;
            this.createTransactionDtoValidator = createTransactionDtoValidator;
            this.updateTransactionDtoValidator = updateTransactionDtoValidator;
            this.errorMessageProvider = errorMessageProvider;
            this.logger = logger;
        }

        public async Task<ServiceResult> ExistsAsync(Guid id)
        {
            this.logger.LogInformation("Validating the existence of transaction {id}", id);
            var result = await this.transactionsProvider.GetByIdAsync(id);
            if (result != null)
            {
                this.logger.LogInformation("Transaction with {id} exists", id);
                return ServiceResult.Success;
            }

            var errorMessage = this.errorMessageProvider.NotFoundMessage(MESSAGE_LABEL, id);
            this.logger.LogWarning("Validation error : {message}", errorMessage);
            return new NotFoundError(errorMessage);
        }

        public async Task<ServiceResult> ValidateAsync(CreateTransactionDto createTransactionDto)
        {
            this.logger.LogInformation("Validating Create Transaction data");
            var result = await this.createTransactionDtoValidator.ValidateAsync(createTransactionDto);
            if (!result.IsValid)
            {
                var errorMessage = errorMessageProvider.ValidationMessage(MESSAGE_LABEL);
                this.logger.LogWarning("Validation error : {message}", errorMessage);

                var fieldErrors = result.Errors.ToFieldValidationError();
                this.logger.LogTrace("Validation field errors : {fieldErrors}", fieldErrors);

                return new ValidationError(message: errorMessage, errors: fieldErrors);
            }

            this.logger.LogInformation("Create Transaction data validated");
            return ServiceResult.Success;
        }

        public async Task<ServiceResult> ValidateAsync(UpdateTransactionDto updateTransactionDto)
        {
            this.logger.LogInformation("Validating Update Transaction data");
            var result = await this.updateTransactionDtoValidator.ValidateAsync(updateTransactionDto);
            if (!result.IsValid)
            {
                var errorMessage = errorMessageProvider.ValidationMessage(MESSAGE_LABEL);
                this.logger.LogWarning("Validation error : {message}", errorMessage);

                var fieldErrors = result.Errors.ToFieldValidationError();
                this.logger.LogTrace("Validation field errors : {fieldErrors}", fieldErrors);

                return new ValidationError(message: errorMessage, errors: fieldErrors);
            }

            this.logger.LogInformation("Create Update Transaction data validated");
            return ServiceResult.Success;
        }
    }
}
