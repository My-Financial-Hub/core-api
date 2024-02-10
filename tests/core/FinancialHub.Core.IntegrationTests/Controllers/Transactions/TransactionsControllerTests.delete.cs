using FinancialHub.Core.Domain.Enums;

namespace FinancialHub.Core.IntegrationTests.Controllers.Transactions
{
    public partial class TransactionsControllerTests
    {
        [Test]
        public async Task Delete_ReturnNoContent()
        {
            var body = InsertTransaction();

            var response = await client.DeleteAsync($"{baseEndpoint}/{body.Id}");

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        public async Task Delete_PaidTransaction_RemovesTransactionFromDatabase()
        {
            var body = modelBuilder
                .WithStatus(TransactionStatus.Committed)
                .WithActiveStatus(true)
                .Generate();

            body.Id = InsertTransaction(body);

            await client.DeleteAsync($"{baseEndpoint}/{body.Id}");

            var result = fixture.GetData<TransactionEntity>();
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public async Task Delete_EarnPaidTransaction_DecreasesBalanceFromDatabase()
        {
            var body = modelBuilder
                .WithStatus(TransactionStatus.Committed)
                .WithType(TransactionType.Earn)
                .WithActiveStatus(true)
                .Generate();

            body.Id = InsertTransaction(body);
            var oldBalanceAmount = fixture.GetData<BalanceEntity>().FirstOrDefault(x => x.Id == body.BalanceId)!.Amount;

            await client.DeleteAsync($"{baseEndpoint}/{body.Id}");

            var balance = fixture.GetData<BalanceEntity>().FirstOrDefault(x => x.Id == body.BalanceId);
            Assert.That(balance!.Amount, Is.EqualTo(oldBalanceAmount - body.Amount));
        }

        [Test]
        public async Task Delete_ExpensePaidTransaction_DecreasesBalanceFromDatabase()
        {
            var body = modelBuilder
                .WithStatus(TransactionStatus.Committed)
                .WithType(TransactionType.Expense)
                .WithActiveStatus(true)
                .Generate();

            body.Id = InsertTransaction(body);
            var oldBalanceAmount = fixture.GetData<BalanceEntity>().FirstOrDefault(x => x.Id == body.BalanceId)!.Amount;

            await client.DeleteAsync($"{baseEndpoint}/{body.Id}");

            var balance = fixture.GetData<BalanceEntity>().FirstOrDefault(x => x.Id == body.BalanceId);
            Assert.That(balance!.Amount, Is.EqualTo(oldBalanceAmount + body.Amount));
        }

        [TestCase(TransactionStatus.NotCommitted, true)]
        [TestCase(TransactionStatus.NotCommitted, false)]
        [TestCase(TransactionStatus.Committed, false)]
        public async Task Delete_NotPaidTransaction_RemovesTransactionFromDatabase(TransactionStatus status, bool isActive)
        {
            var body = modelBuilder
                .WithStatus(status)
                .WithActiveStatus(isActive)
                .Generate();

            body.Id = InsertTransaction(body);

            await client.DeleteAsync($"{baseEndpoint}/{body.Id}");

            var result = fixture.GetData<TransactionEntity>();
            Assert.AreEqual(0, result.Count());
        }
    }
}
