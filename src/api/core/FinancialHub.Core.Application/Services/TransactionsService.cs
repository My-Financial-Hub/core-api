using AutoMapper;
using FinancialHub.Core.Domain.DTOS.Transactions;
using FinancialHub.Core.Domain.Filters;
using FinancialHub.Core.Domain.Interfaces.Validators;

namespace FinancialHub.Core.Application.Services
{
    public class TransactionsService : ITransactionsService
    {
        private readonly ITransactionsProvider transactionsProvider;
        private readonly ITransactionsValidator transactionsValidator;
        private readonly ICategoriesValidator categoriesValidator;
        private readonly IBalancesValidator balancesValidator;
        private readonly IMapper mapper;

        public TransactionsService(
            ITransactionsProvider transactionsProvider,
            IBalancesValidator balancesValidator,
            ITransactionsValidator transactionsValidator,
            ICategoriesValidator categoriesValidator,
            IMapper mapper
        )
        {
            this.transactionsProvider = transactionsProvider;

            this.transactionsValidator = transactionsValidator;
            this.categoriesValidator = categoriesValidator;
            this.balancesValidator = balancesValidator;

            this.mapper = mapper;
        }

        private async Task<ServiceResult> ValidateTransaction(TransactionModel transaction)
        {
            var balance = await this.balancesValidator.ExistsAsync(transaction.BalanceId);
            if (balance.HasError)
            {
                return balance.Error;
            }

            var category = await this.categoriesValidator.ExistsAsync(transaction.CategoryId);
            if (category.HasError)
            {
                return category.Error;
            }

            return ServiceResult.Success;
        }

        public async Task<ServiceResult<TransactionDto>> CreateAsync(CreateTransactionDto transaction)
        {
            var validation = await this.transactionsValidator.ValidateAsync(transaction);
            if (validation.HasError)
            {
                return validation.Error;
            }

            var transactionModel = this.mapper.Map<TransactionModel>(transaction);

            validation = await this.ValidateTransaction(transactionModel);
            if (validation.HasError)
            {
                return validation.Error;
            }

            var createdTransaction = await this.transactionsProvider.CreateAsync(transactionModel);
            
            return this.mapper.Map<TransactionDto>(createdTransaction);
        }

        public async Task<ServiceResult<int>> DeleteAsync(Guid id)
        {
            return await this.transactionsProvider.DeleteAsync(id);
        }

        public async Task<ServiceResult<ICollection<TransactionDto>>> GetAllAsync(TransactionFilter filter)
        {
            var transactions = await this.transactionsProvider.GetAllAsync(filter);

            return this.mapper.Map<ICollection<TransactionDto>>(transactions).ToArray();
        }

        public async Task<ServiceResult<TransactionDto>> UpdateAsync(Guid id, UpdateTransactionDto transaction)
        {
            var validation = await this.transactionsValidator.ValidateAsync(transaction);
            if (validation.HasError)
            {
                return validation.Error;
            }

            validation = await this.transactionsValidator.ExistsAsync(id);
            if (validation.HasError)
            {
                return validation.Error;
            }

            var transactionModel = this.mapper.Map<TransactionModel>(transaction);

            validation = await this.ValidateTransaction(transactionModel);
            if (validation.HasError)
            {
                return validation.Error;
            }

            var updatedTransaction = await this.transactionsProvider.UpdateAsync(id, transactionModel);
            return this.mapper.Map<TransactionDto>(updatedTransaction);
        }

        public async Task<ServiceResult<TransactionDto>> GetByIdAsync(Guid id)
        {
            var exists = await this.transactionsValidator.ExistsAsync(id);
            if (exists.HasError)
            {
                return exists.Error;
            }

            var transaction = await this.transactionsProvider.GetByIdAsync(id);
            return this.mapper.Map<TransactionDto>(transaction);
        }
    }
}
