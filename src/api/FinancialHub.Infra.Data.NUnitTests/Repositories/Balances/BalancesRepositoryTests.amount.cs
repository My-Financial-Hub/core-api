using FinancialHub.Domain.Enums;

namespace FinancialHub.Infra.Data.NUnitTests.Repositories
{
    public partial class BalancesRepositoryTests
    {
        [Test]
        public async Task AddAmountAsync_EarnTransaction_AddsAmount()
        {
            var balanceId = Guid.NewGuid();
            var balance = this.balanceBuilder
                .WithAmount(0)
                .WithId(balanceId)
                .Generate();
            await this.InsertData(balance);
            this.context.ChangeTracker.Clear();

            var amount = this.random.Next(10,1000);
            var entity = this.transactionBuilder
                .WithType(TransactionType.Earn)
                .WithBalanceId(balanceId)
                .WithAmount(amount)
                .Generate();

            var balanceResult = await this.balanceRepository.ChangeAmountAsync(balanceId,amount, TransactionType.Earn);
            Assert.AreEqual(balance.Amount + entity.Amount, balanceResult.Amount);
        }

        [Test]
        public async Task AddAmountAsync_ExpenseTransaction_RemovesAmount()
        {
            var balanceId = Guid.NewGuid();
            var balance = this.balanceBuilder
                .WithAmount(0)
                .WithId(balanceId)
                .Generate();
            await this.InsertData(balance);
            this.context.ChangeTracker.Clear();

            var amount = this.random.Next(10, 1000);
            var entity = this.transactionBuilder
                .WithType(TransactionType.Expense)
                .WithBalanceId(balanceId)
                .WithAmount(amount)
                .Generate();

            var balanceResult = await this.balanceRepository.ChangeAmountAsync(balanceId, amount, TransactionType.Expense);
            Assert.AreEqual(balance.Amount - entity.Amount, balanceResult.Amount);
        }

        [Test]
        public async Task RemoveAmountAsync_EarnTransaction_RemovesAmount()
        {
            var balanceId = Guid.NewGuid();
            var balance = this.balanceBuilder
                .WithAmount(0)
                .WithId(balanceId)
                .Generate();
            await this.InsertData(balance);
            this.context.ChangeTracker.Clear();

            var amount = this.random.Next(10, 1000);
            var entity = this.transactionBuilder
                .WithType(TransactionType.Earn)
                .WithBalanceId(balanceId)
                .WithAmount(amount)
                .Generate();

            var balanceResult = await this.balanceRepository.ChangeAmountAsync(balanceId, amount, TransactionType.Earn, true);
            Assert.AreEqual(balance.Amount - entity.Amount, balanceResult.Amount);
        }

        [Test]
        public async Task RemoveAmountAsync_ExpenseTransaction_AddsAmount()
        {
            var balanceId = Guid.NewGuid();
            var balance = this.balanceBuilder
                .WithAmount(0)
                .WithId(balanceId)
                .Generate();
            await this.InsertData(balance);
            this.context.ChangeTracker.Clear();

            var amount = this.random.Next(10, 1000);
            var entity = this.transactionBuilder
                .WithType(TransactionType.Expense)
                .WithBalanceId(balanceId)
                .WithAmount(amount)
                .Generate();

            var balanceResult = await this.balanceRepository.ChangeAmountAsync(balanceId, amount, TransactionType.Expense, true);
            Assert.AreEqual(balance.Amount + entity.Amount, balanceResult.Amount);
        }
    }
}
