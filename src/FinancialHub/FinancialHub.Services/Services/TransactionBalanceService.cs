using FinancialHub.Domain.Interfaces.Services;
using FinancialHub.Domain.Models;
using FinancialHub.Domain.Results;
using FinancialHub.Domain.Enums;

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

                if (transaction.Type == TransactionType.Earn)
                {
                    await balancesService.UpdateAmountAsync(transaction.BalanceId, balanceResult.Data.Amount + transaction.Amount);
                }
                else
                {
                    await balancesService.UpdateAmountAsync(transaction.BalanceId, balanceResult.Data.Amount - transaction.Amount);
                }
            }

            return transactionResult;
        }

        public async Task<ServiceResult<TransactionModel>> UpdateTransactionAsync(Guid id, TransactionModel transaction)
        {
            var oldTransactionResult = await transactionsService.GetByIdAsync(id);

            var transactionResult = await transactionsService.UpdateAsync(id, transaction);
            if (transactionResult.HasError)
            {
                return transactionResult;
            }

            var oldTransaction = oldTransactionResult.Data;
            var newTransaction = transactionResult.Data;
            var balanceResult = await balancesService.GetByIdAsync(newTransaction.BalanceId);

            if (newTransaction.BalanceId == oldTransaction.BalanceId)
            {
                oldTransaction.Balance = balanceResult.Data;
                newTransaction.Balance = balanceResult.Data;
            }
            else
            {
                var oldBalanceResult = await balancesService.GetByIdAsync(oldTransaction.BalanceId);

                oldTransaction.Balance = oldBalanceResult.Data;
                newTransaction.Balance = balanceResult.Data;
            }
            await this.UpdateAmountAsync(oldTransaction, newTransaction);

            return transactionResult;
        }

        public async Task<ServiceResult<bool>> DeleteTransactionAsync(Guid id)
        {
            var oldTransaction = await this.transactionsService.GetByIdAsync(id);
            
            var deleted = await this.transactionsService.DeleteAsync(id);

            if (deleted.HasError)
                return deleted.Error;
            if (deleted.Data == 0)
                return false;

            var transaction = oldTransaction.Data;
            if (transaction.IsPaid)
            {
                var amount = transaction.Balance.Amount;
                amount = transaction.Type == TransactionType.Earn?
                    amount - transaction.Amount:
                    amount + transaction.Amount;

                await this.balancesService.UpdateAmountAsync(transaction.BalanceId, amount);
            }

            return true;
        }
    }
}
