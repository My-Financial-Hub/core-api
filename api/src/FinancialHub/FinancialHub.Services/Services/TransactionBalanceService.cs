using FinancialHub.Domain.Models;
using FinancialHub.Domain.Results;
using FinancialHub.Domain.Interfaces.Services;

namespace FinancialHub.Services.Services
{
    public class TransactionBalanceService : ITransactionBalanceService
    {
        private readonly ITransactionsService transactionsService;
        private readonly IBalancesService balancesService;

        public TransactionBalanceService(ITransactionsService transactionsService, IBalancesService balancesService)
        {
            this.transactionsService = transactionsService;
            this.balancesService = balancesService;
        }

        public async Task<ServiceResult<TransactionModel>> CreateTransactionAsync(TransactionModel transaction)
        {
            var transactionResult = await transactionsService.CreateAsync(transaction);

            if(transactionResult.HasError) { 
                return transactionResult;
            }

            if(transactionResult.Data.IsPaid)
            {
                var balanceResult = await balancesService.GetByIdAsync(transaction.BalanceId);
                if(balanceResult.HasError) { 
                    return balanceResult.Error;
                }

                decimal newAmount;
                if (transaction.Type == Domain.Enums.TransactionType.Earn)
                {
                    newAmount = balanceResult.Data.Amount + transaction.Amount;
                }
                else
                {
                    newAmount = balanceResult.Data.Amount - transaction.Amount;
                }

                await balancesService.UpdateAmountAsync(transaction.BalanceId, newAmount);
            }

            return transactionResult;
        }
    }
}
