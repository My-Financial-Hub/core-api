using FinancialHub.Core.Domain.DTOS.Transactions;

namespace FinancialHub.Core.IntegrationTests.Controllers.Transactions
{
    public partial class TransactionsControllerTests
    {
        [Test]
        public async Task GetAll_ReturnActiveTransactions()
        {
            var entities = transactionBuilder.Generate();

            fixture.AddData(entities.Category);
            fixture.AddData(entities.Balance);

            var transactions = transactionBuilder
                .WithBalanceId(entities.Balance.Id)
                .WithCategoryId(entities.Category.Id)
                .WithActiveStatus(true)
                .Generate(10);

            fixture.AddData(transactions.ToArray());

            var response = await client.GetAsync(baseEndpoint);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<ListResponse<TransactionDto>>();
            Assert.AreEqual(transactions.Count, result?.Data.Count);
        }

        [Test]
        public async Task GetAll_DoesNotReturnNotActiveTransactions()
        {
            var entities = transactionBuilder.Generate();

            fixture.AddData(entities.Category);
            fixture.AddData(entities.Balance);

            var transactions = transactionBuilder
                .WithBalanceId(entities.Balance.Id)
                .WithCategoryId(entities.Category.Id)
                .WithActiveStatus(false)
                .Generate(10);

            fixture.AddData(transactions.ToArray());

            var response = await client.GetAsync(baseEndpoint);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<ListResponse<TransactionDto>>();
            Assert.Zero(result!.Data.Count);
        }
    }
}
