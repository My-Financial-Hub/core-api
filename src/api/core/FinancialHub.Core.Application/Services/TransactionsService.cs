using AutoMapper;
using FinancialHub.Common.Extensions;
using FinancialHub.Core.Domain.DTOS.Categories;
using FinancialHub.Core.Domain.DTOS.Transactions;
using FinancialHub.Core.Domain.Filters;
using FinancialHub.Core.Domain.Interfaces.Validators;
using Microsoft.Extensions.Logging;
using System.Security.Principal;

namespace FinancialHub.Core.Application.Services
{
    public class TransactionsService : ITransactionsService
    {
        private readonly ITransactionsProvider transactionsProvider;
        private readonly ITransactionsValidator transactionsValidator;
        private readonly ICategoriesValidator categoriesValidator;
        private readonly IBalancesValidator balancesValidator;
        private readonly IMapper mapper;
        private readonly ILogger<TransactionsService> logger;

        public TransactionsService(
            ITransactionsProvider transactionsProvider,
            ITransactionsValidator transactionsValidator,
            IBalancesValidator balancesValidator,
            ICategoriesValidator categoriesValidator,
            IMapper mapper,
            ILogger<TransactionsService> logger
        )
        {
            this.transactionsProvider = transactionsProvider;
            this.transactionsValidator = transactionsValidator;

            this.categoriesValidator = categoriesValidator;
            this.balancesValidator = balancesValidator;

            this.mapper = mapper;
            this.logger = logger;
        }

        private async Task<ServiceResult> ValidateTransaction(TransactionModel transaction)
        {
            var validation = await this.balancesValidator.ExistsAsync(transaction.BalanceId);
            if (validation.HasError)
            {
                return validation.Error;
            }

            validation = await this.categoriesValidator.ExistsAsync(transaction.CategoryId);
            if (validation.HasError)
            {
                return validation.Error;
            }

            return ServiceResult.Success;
        }

        public async Task<ServiceResult<TransactionDto>> CreateAsync(CreateTransactionDto transaction)
        {
            this.logger.LogInformation("Creating transaction {type}", transaction.Type);
            this.logger.LogTrace("Transaction data : {category}", transaction.ToJson());

            var validationResult = await this.transactionsValidator.ValidateAsync(transaction);
            if (validationResult.HasError)
            {
                this.logger.LogTrace("Transaction creation Validation result : {validationResult}", validationResult.ToJson());
                this.logger.LogInformation("Failed creating transaction {type}", transaction.Type);
                return validationResult.Error;
            }

            var transactionModel = this.mapper.Map<TransactionModel>(transaction);

            validationResult = await this.ValidateTransaction(transactionModel);
            if (validationResult.HasError)
            {
                this.logger.LogTrace("Transaction creation Validation result : {validationResult}", validationResult.ToJson());
                this.logger.LogInformation("Failed creating transaction {type}", transaction.Type);
                return validationResult.Error;
            }

            var createdTransaction = await this.transactionsProvider.CreateAsync(transactionModel);
            
            var result = this.mapper.Map<TransactionDto>(createdTransaction);

            this.logger.LogTrace("Transaction creation result : {result}", result.ToJson());
            this.logger.LogInformation("Transaction {type} Sucessfully created", result.Type);

            return result;
        }

        public async Task<ServiceResult<int>> DeleteAsync(Guid id)
        {
            this.logger.LogInformation("Removing transaction {id}", id);
            var amount = await this.transactionsProvider.DeleteAsync(id);
            this.logger.LogInformation("Transaction {id} {removed}", id, amount > 0 ? "removed" : "not removed");
            return amount;
        }

        public async Task<ServiceResult<ICollection<TransactionDto>>> GetAllAsync(TransactionFilter filter)
        {
            this.logger.LogInformation("Getting transactions with filter : {filter}", filter.ToJson());
            var transactions = await this.transactionsProvider.GetAllAsync(filter);

            this.logger.LogInformation("Returning {count} transactions", transactions.Count > 0 ? $"{transactions.Count}" : "no");
            return this.mapper.Map<ICollection<TransactionDto>>(transactions).ToArray();
        }

        public async Task<ServiceResult<TransactionDto>> UpdateAsync(Guid id, UpdateTransactionDto transaction)
        {
            var validationResult = await this.transactionsValidator.ValidateAsync(transaction);
            if (validationResult.HasError)
            {
                this.logger.LogTrace("Transaction update validation result : {validationResult}", validationResult.ToJson());
                this.logger.LogInformation("Failed update transaction {id}", id);
                return validationResult.Error;
            }

            validationResult = await this.transactionsValidator.ExistsAsync(id);
            if (validationResult.HasError)
            {
                this.logger.LogTrace("Transaction update validation result : {validationResult}", validationResult.ToJson());
                this.logger.LogInformation("Failed update transaction {id}", id);
                return validationResult.Error;
            }

            var transactionModel = this.mapper.Map<TransactionModel>(transaction);

            validationResult = await this.ValidateTransaction(transactionModel);
            if (validationResult.HasError)
            {
                this.logger.LogTrace("Transaction update validation result : {validationResult}", validationResult.ToJson());
                this.logger.LogInformation("Failed update transaction {id}", id);
                return validationResult.Error;
            }

            var updatedTransaction = await this.transactionsProvider.UpdateAsync(id, transactionModel);
            
            var result = this.mapper.Map<TransactionDto>(updatedTransaction);

            this.logger.LogTrace("Transaction update result : {result}", result.ToJson());
            this.logger.LogInformation("Transaction {id} Sucessfully Updated", id);

            return result;
        }

        public async Task<ServiceResult<TransactionDto>> GetByIdAsync(Guid id)
        {
            this.logger.LogInformation("Getting transaction {id}", id);
            var validationResult = await this.transactionsValidator.ExistsAsync(id);
            if (validationResult.HasError)
            {
                this.logger.LogTrace("Transaction get by id result : {validationResult}", validationResult.ToJson());
                this.logger.LogInformation("Failed getting transaction {id}", id);
                return validationResult.Error;
            }

            var transaction = await this.transactionsProvider.GetByIdAsync(id);
            
            var result = this.mapper.Map<TransactionDto>(transaction);

            this.logger.LogTrace("Transaction result {result}", result.ToJson());
            this.logger.LogInformation("Transaction {id} found", id);
            return result;
        }
    }
}
