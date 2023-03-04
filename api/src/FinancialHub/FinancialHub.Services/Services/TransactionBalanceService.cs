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

        public async Task UpdateAmountAsync(TransactionModel transaction, BalanceModel balance, bool undo = false)
        {
            decimal newAmount = balance.Amount;

            if(!undo && !transaction.IsPaid)
            {
                return;
            }

            if (
                transaction.Type == TransactionType.Earn && !undo ||
                transaction.Type == TransactionType.Expense && undo
            )
            {
                newAmount += transaction.Amount;
            }
            else
            {
                newAmount -= transaction.Amount;
            }

            await balancesService.UpdateAmountAsync(transaction.BalanceId, newAmount);
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

                await this.UpdateAmountAsync(transaction, balanceResult.Data);
            }

            return transactionResult;
        }

        //TODO: improve this code (and architecture)
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
                var oldAmount = balanceResult.Data.Amount;
                var newAmount = balanceResult.Data.Amount;

                if (oldTransaction.IsPaid != newTransaction.IsPaid)
                {
                     await this.UpdateAmountAsync(newTransaction, balanceResult.Data);

                    if (newTransaction.IsPaid)
                    {
                        //await balancesService.UpdateAmountAsync(newTransaction.BalanceId, oldAmount + newTransaction.Amount);
                    }
                    else
                    {
                        //await balancesService.UpdateAmountAsync(newTransaction.BalanceId, oldAmount - oldTransaction.Amount);
                    }
                }
                else if (oldAmount != newAmount && newTransaction.IsPaid)
                {
                    var difference = oldTransaction.Amount - newTransaction.Amount;
                    await balancesService.UpdateAmountAsync(newTransaction.BalanceId, oldAmount + difference);
                }
            }
            else
            {
                var oldBalanceResult = await balancesService.GetByIdAsync(oldTransaction.BalanceId);

                if (oldTransaction.IsPaid)
                {
                    await this.UpdateAmountAsync(oldTransaction, oldBalanceResult.Data);
                    //await balancesService.UpdateAmountAsync(oldTransaction.BalanceId, oldAmount - oldTransaction.Amount);
                    if (newTransaction.IsPaid)
                    {
                        await this.UpdateAmountAsync(newTransaction, balanceResult.Data);
                        //await balancesService.UpdateAmountAsync(newTransaction.BalanceId, newAmount + newTransaction.Amount);
                    }
                }
                else if(newTransaction.IsPaid)
                {
                    await this.UpdateAmountAsync(newTransaction, balanceResult.Data);
                    //await balancesService.UpdateAmountAsync(newTransaction.BalanceId, newAmount + newTransaction.Amount);
                }
            }

            return transactionResult;
        }
    }
}
