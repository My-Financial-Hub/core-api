using FinancialHub.Core.Domain.DTOS.Transactions;
using FinancialHub.Core.Domain.Tests.Builders.DTOS.Transactions;

namespace FinancialHub.Core.IntegrationTests.Controllers.Transactions
{
    public partial class TransactionsControllerTests
    {
        private UpdateTransactionDtoBuilder updateTransactionDtoBuilder;
        protected void AddUpdateTransactionBuilder()
        {
            updateTransactionDtoBuilder = new UpdateTransactionDtoBuilder();
        }

        protected TransactionEntity GetBalance(UpdateTransactionDto transaction)
        {
            return fixture
                .GetData<TransactionEntity>()
                .First(
                    tra =>
                        tra.BalanceId == transaction.BalanceId &&
                        tra.CategoryId == transaction.CategoryId &&
                        tra.TargetDate == transaction.TargetDate &&
                        tra.FinishDate == transaction.FinishDate &&
                        tra.Amount == transaction.Amount &&
                        tra.Type == transaction.Type &&
                        tra.Description == transaction.Description &&
                        tra.IsActive == transaction.IsActive
                );
        }
        
        [Test]
        [Ignore("endpoint disabled")]
        public async Task Put_ExistingTransaction_ReturnUpdatedTransaction()
        {
            var data = InsertTransaction(true);

            var body = updateTransactionDtoBuilder
                .WithBalanceId(data.BalanceId)
                .WithCategoryId(data.CategoryId)
                .WithActiveStatus(data.IsActive)
                .Generate();

            var response = await client.PutAsync($"{baseEndpoint}/{data.Id}", body);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.ReadContentAsync<SaveResponse<TransactionDto>>();
            Assert.IsNotNull(result?.Data);
            var resultData = result!.Data;
            Assert.AreEqual(data.BalanceId, resultData.Balance.Id);
            Assert.AreEqual(data.CategoryId, resultData.Category.Id);
            Assert.AreEqual(data.TargetDate, resultData.TargetDate);
            Assert.AreEqual(data.FinishDate, resultData.FinishDate);
            Assert.AreEqual(data.Amount, resultData.Amount);
            Assert.AreEqual(data.Type, resultData.Type);
            Assert.AreEqual(data.Description, resultData.Description);
            Assert.AreEqual(data.IsActive, resultData.IsActive);
        }

        [Test]
        [Ignore("endpoint disabled")]
        public async Task Put_ExistingTransaction_UpdatesTransaction()
        {
            var data = InsertTransaction(true);

            var body = updateTransactionDtoBuilder
                .WithBalanceId(data.BalanceId)
                .WithCategoryId(data.CategoryId)
                .WithActiveStatus(data.IsActive)
                .Generate();
            await client.PutAsync($"{baseEndpoint}/{data.Id}", body);

            var balance = GetBalance(body);
            Assert.IsNotNull(balance);
        }

        [Test]
        [Ignore("endpoint disabled")]
        public async Task Put_NonExistingTransaction_ReturnNotFoundError()
        {
            var id = Guid.NewGuid();
            var body = updateTransactionDtoBuilder.Generate();

            var response = await client.PutAsync($"{baseEndpoint}/{id}", body);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var result = await response.ReadContentAsync<NotFoundErrorResponse>();
            Assert.AreEqual($"Not found Transaction with id {id}", result!.Message);
        }
    }
}
