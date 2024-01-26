using FinancialHub.Core.Domain.Enums;

namespace FinancialHub.Core.Application.Services
{
    [Obsolete("This Service will be removed and the logic will be moved to Balance and Transaction models")]
    public class TransactionBalanceService : ITransactionBalanceService
    {
        private readonly ITransactionsService transactionsService;
        private readonly IBalancesService balancesService;

        public TransactionBalanceService(ITransactionsService transactionsService, IBalancesService balancesService)
        {
            this.transactionsService = transactionsService;
            this.balancesService = balancesService;
        }

        private static (decimal oldAmount, decimal newAmount) UpdateAmountDifferentBalances(TransactionModel oldTransaction, TransactionModel newTransaction)
        {
            var oldAmount = oldTransaction.Balance.Amount; 
            var newAmount = newTransaction.Balance.Amount;

            if (oldTransaction.IsPaid)
            {
                oldAmount =
                    oldTransaction.Type == TransactionType.Earn ?
                    oldAmount - oldTransaction.Amount :
                    oldAmount + oldTransaction.Amount;
            }

            if (newTransaction.IsPaid)
            {
                newAmount =
                    newTransaction.Type == TransactionType.Earn ?
                    newAmount + newTransaction.Amount :
                    newAmount - newTransaction.Amount;
            }

            return (oldAmount, newAmount);
        }

        private static decimal UpdateAmountDifferentStatus(TransactionModel newTransaction)
        {
            var newAmount = newTransaction.Balance.Amount;

            if (newTransaction.IsPaid)
            {
                if (newTransaction.Type == TransactionType.Earn)
                    return newAmount + newTransaction.Amount;
                else
                    return newAmount - newTransaction.Amount;
            }
            else
            {
                if (newTransaction.Type == TransactionType.Earn)
                    return newAmount - newTransaction.Amount;
                else
                    return newAmount + newTransaction.Amount;
            }
        }

        private static decimal UpdateAmountSameStatus(TransactionModel oldTransaction, TransactionModel newTransaction)
        {
            var newAmount = newTransaction.Balance.Amount;

            if (newTransaction.Type == oldTransaction.Type)
            {
                decimal difference;
                if (oldTransaction.Type == TransactionType.Earn)
                    difference = newTransaction.Amount - oldTransaction.Amount;
                else
                    difference = oldTransaction.Amount - newTransaction.Amount;

                return newAmount + difference;
            }
            else
            {
                var difference = oldTransaction.Amount + newTransaction.Amount;
                if (oldTransaction.Type == TransactionType.Earn)
                    return newAmount - difference;
                else
                    return newAmount + difference;
            }
        }

        private static decimal UpdateAmountSameBalance(TransactionModel oldTransaction, TransactionModel newTransaction)
        {
            var newAmount = newTransaction.Balance.Amount;
            if (oldTransaction.IsPaid != newTransaction.IsPaid)
            {
                return UpdateAmountDifferentStatus(newTransaction);
            }
            else if (oldTransaction.IsPaid && newTransaction.IsPaid)
            {
                return UpdateAmountSameStatus(oldTransaction, newTransaction);
            }
            return newAmount;
        }

        public async Task UpdateAmountAsync(TransactionModel oldTransaction, TransactionModel newTransaction)
        {
            if(oldTransaction.IsPaid || newTransaction.IsPaid)
            {
                if (oldTransaction.BalanceId != newTransaction.BalanceId)
                {
                    var (oldAmount, newAmount) = UpdateAmountDifferentBalances(oldTransaction, newTransaction);

                    if (oldAmount != oldTransaction.Balance.Amount)
                        await this.balancesService.UpdateAmountAsync(oldTransaction.BalanceId, oldAmount);
                    if (newAmount != newTransaction.Balance.Amount)
                        await this.balancesService.UpdateAmountAsync(newTransaction.BalanceId, newAmount);
                }
                else
                {
                    var newAmount = UpdateAmountSameBalance(oldTransaction, newTransaction);

                    if(newAmount != newTransaction.Balance.Amount)
                    {
                        await this.balancesService.UpdateAmountAsync(newTransaction.BalanceId, newAmount);
                    }
                }
            }
        }

        public async Task<ServiceResult<TransactionModel>> UpdateTransactionAsync(Guid id, TransactionModel transaction)
        {
            var oldTransactionResult = await transactionsService.GetByIdAsync(id);

            var transactionResult = await transactionsService.UpdateAsync(id, transaction);
            if (transactionResult.HasError)
            {
                return transactionResult;
            }

            var oldTransaction = oldTransactionResult.Data!;
            var newTransaction = transactionResult.Data!;
            var balanceResult = await balancesService.GetByIdAsync(newTransaction.BalanceId);

            if (newTransaction.BalanceId == oldTransaction.BalanceId)
            {
                //oldTransaction.Balance = balanceResult.Data;
                //newTransaction.Balance = balanceResult.Data;
            }
            else
            {
                //var oldBalanceResult = await balancesService.GetByIdAsync(oldTransaction.BalanceId);
                
                //oldTransaction.Balance = oldBalanceResult.Data;
                //newTransaction.Balance = balanceResult.Data;
            }
            await this.UpdateAmountAsync(oldTransaction, newTransaction);

            return transactionResult;
        }
    }
}
