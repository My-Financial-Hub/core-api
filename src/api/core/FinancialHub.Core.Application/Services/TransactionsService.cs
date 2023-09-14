using FinancialHub.Core.Domain.Filters;
using FinancialHub.Core.Domain.Enums;

namespace FinancialHub.Core.Application.Services
{
    public class TransactionsService : ITransactionsService
    {
        private readonly ITransactionsProvider transactionsProvider;
        private readonly IBalancesProvider balancesProvider;
        private readonly ICategoriesProvider categoriesProvider;

        public TransactionsService(
            ITransactionsProvider transactionsProvider,
            IBalancesProvider balancesProvider, ICategoriesProvider categoriesProvider
        )
        {
            this.transactionsProvider = transactionsProvider;
            this.balancesProvider = balancesProvider;
            this.categoriesProvider = categoriesProvider;
        }

        private async Task<ServiceResult<bool>> ValidateTransaction(TransactionModel transaction)
        {
            var balance = await this.balancesProvider.GetByIdAsync(transaction.BalanceId);
            if (balance == null)
                return new NotFoundError($"Not found Balance with id {transaction.BalanceId}"); 

            var category = await this.categoriesProvider.GetByIdAsync(transaction.CategoryId);
            if (category == null)
                return new NotFoundError($"Not found Category with id {transaction.CategoryId}");

            return true;
        }

        public async Task<ServiceResult<TransactionModel>> CreateAsync(TransactionModel transaction)
        {
            var validation = await this.ValidateTransaction(transaction);
            if (validation.HasError)
                return new ServiceResult<TransactionModel>(error: validation.Error);

            return await this.transactionsProvider.CreateAsync(transaction);
        }

        public async Task<ServiceResult<int>> DeleteAsync(Guid id)
        {
            var transactionResult = await this.GetByIdAsync(id);
            if (transactionResult.HasError)
                return transactionResult.Error;

            var transaction = transactionResult.Data!;

            if (transaction.Status == TransactionStatus.Committed && transaction.IsActive)
            {
                await this.balancesProvider.DecreaseAmountAsync(transaction.BalanceId, transaction.Amount, transaction.Type);
            }

            return await this.transactionsProvider.DeleteAsync(id);
        }

        public async Task<ServiceResult<ICollection<TransactionModel>>> GetAllByUserAsync(string userId, TransactionFilter filter)
        {
            var transactions = await this.transactionsProvider.GetAllAsync(filter);

            return transactions.ToArray();
        }

        public async Task<ServiceResult<TransactionModel>> UpdateAsync(Guid id, TransactionModel transaction)
        {
            var oldTransactionResult = await this.GetByIdAsync(id);
            if (oldTransactionResult.HasError)
                return oldTransactionResult.Error;

            var validation = await this.ValidateTransaction(transaction);
            if (validation.HasError)
                return new ServiceResult<TransactionModel>(error: validation.Error);

            return await this.transactionsProvider.UpdateAsync(id, transaction);
        }

        public async Task<ServiceResult<TransactionModel>> GetByIdAsync(Guid id)
        {
            var transaction = await this.transactionsProvider.GetByIdAsync(id);
            if (transaction == null)
                return new NotFoundError($"Not found Transaction with id {id}");

            return transaction;
        }
    }
}
