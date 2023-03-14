using FinancialHub.Domain.Models;
using FinancialHub.Domain.Results;
using FinancialHub.Domain.Interfaces.Services;
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

        private static decimal UpdateAmountSameBalance(TransactionModel oldTransaction, TransactionModel newTransaction)
        {
            var newAmount = newTransaction.Balance.Amount;
            if (oldTransaction.IsPaid != newTransaction.IsPaid)
            {
                if (newTransaction.IsPaid)
                {
                    newAmount =
                        newTransaction.Type == TransactionType.Earn ?
                        newAmount + newTransaction.Amount :
                        newAmount - newTransaction.Amount;
                }
                else
                {
                    newAmount =
                       newTransaction.Type == TransactionType.Earn ?
                       newAmount - newTransaction.Amount :
                       newAmount + newTransaction.Amount;
                }
            }
            else if (oldTransaction.IsPaid && newTransaction.IsPaid)
            {
                if (newTransaction.Type == oldTransaction.Type)
                {
                    var difference =
                        oldTransaction.Type == TransactionType.Earn ?
                        newTransaction.Amount - oldTransaction.Amount :
                        oldTransaction.Amount - newTransaction.Amount;
                    newAmount += difference;
                }
                else
                {
                    var difference = oldTransaction.Amount + newTransaction.Amount;
                    newAmount =
                        oldTransaction.Type == TransactionType.Earn ?
                        newAmount - difference :
                        newAmount + difference;
                }
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
    }
}
