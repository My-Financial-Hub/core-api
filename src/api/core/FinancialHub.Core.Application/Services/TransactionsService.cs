using AutoMapper;
using FinancialHub.Core.Domain.DTOS.Transactions;
using FinancialHub.Core.Domain.Filters;
using FinancialHub.Core.Domain.Interfaces.Resources;

namespace FinancialHub.Core.Application.Services
{
    public class TransactionsService : ITransactionsService
    {
        private readonly ITransactionsProvider transactionsProvider;
        private readonly IBalancesProvider balancesProvider;
        private readonly ICategoriesProvider categoriesProvider;
        private readonly IMapper mapper;
        private readonly IErrorMessageProvider errorMessageProvider;

        public TransactionsService(
            ITransactionsProvider transactionsProvider,
            IBalancesProvider balancesProvider,
            ICategoriesProvider categoriesProvider,
            IMapper mapper,
            IErrorMessageProvider errorMessageProvider
        )
        {
            this.transactionsProvider = transactionsProvider;
            this.balancesProvider = balancesProvider;
            this.categoriesProvider = categoriesProvider;
            this.mapper = mapper;
            this.errorMessageProvider = errorMessageProvider;
        }

        private async Task<ServiceResult<bool>> ValidateTransaction(TransactionModel transaction)
        {
            var balance = await this.balancesProvider.GetByIdAsync(transaction.BalanceId);
            if (balance == null)
            {
                return new NotFoundError(
                    this.errorMessageProvider.NotFoundMessage("Balance", transaction.BalanceId)
                ); 
            }

            var category = await this.categoriesProvider.GetByIdAsync(transaction.CategoryId);
            if (category == null)
            {
                return new NotFoundError(
                    this.errorMessageProvider.NotFoundMessage("Category", transaction.CategoryId)
                );
            }

            return true;
        }

        public async Task<ServiceResult<TransactionDto>> CreateAsync(CreateTransactionDto transaction)
        {
            var transactionModel = this.mapper.Map<TransactionModel>(transaction);

            var validation = await this.ValidateTransaction(transactionModel);
            if (validation.HasError)
            {
                return new ServiceResult<TransactionDto>(error: validation.Error);
            }

            var createdTransaction = await this.transactionsProvider.CreateAsync(transactionModel);
            return this.mapper.Map<TransactionDto>(createdTransaction);
        }

        public async Task<ServiceResult<int>> DeleteAsync(Guid id)
        {
            var transactionResult = await this.GetByIdAsync(id);
            if (transactionResult.HasError)
            {
                return transactionResult.Error;
            }

            return await this.transactionsProvider.DeleteAsync(id);
        }

        public async Task<ServiceResult<ICollection<TransactionDto>>> GetAllAsync(TransactionFilter filter)
        {
            var transactions = await this.transactionsProvider.GetAllAsync(filter);

            return this.mapper.Map<ICollection<TransactionDto>>(transactions).ToArray();
        }

        public async Task<ServiceResult<TransactionDto>> UpdateAsync(Guid id, UpdateTransactionDto transaction)
        {
            var oldTransactionResult = await this.GetByIdAsync(id);
            if (oldTransactionResult.HasError)
            {
                return oldTransactionResult.Error;
            }

            var transactionModel = this.mapper.Map<TransactionModel>(transaction);
            var validation = await this.ValidateTransaction(transactionModel);
            if (validation.HasError)
            {
                return new ServiceResult<TransactionDto>(error: validation.Error);
            }

            var updatedTransaction = await this.transactionsProvider.UpdateAsync(id, transactionModel);
            return this.mapper.Map<TransactionDto>(updatedTransaction);
        }

        public async Task<ServiceResult<TransactionDto>> GetByIdAsync(Guid id)
        {
            var transaction = await this.transactionsProvider.GetByIdAsync(id);
            if (transaction == null)
            {
                return new NotFoundError(
                    this.errorMessageProvider.NotFoundMessage("Transaction", id)
                );
            }

            return this.mapper.Map<TransactionDto>(transaction);
        }
    }
}
