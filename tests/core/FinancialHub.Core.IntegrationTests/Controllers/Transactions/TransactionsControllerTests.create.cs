using FinancialHub.Core.Domain.DTOS.Transactions;
using FinancialHub.Core.Domain.Tests.Builders.DTOS.Transactions;

namespace FinancialHub.Core.IntegrationTests.Controllers.Transactions
{
    public partial class TransactionsControllerTests
    {
        private CreateTransactionDtoBuilder createTransactionDtoBuilder;
        protected void AddCreateTransactionBuilder()
        {
            createTransactionDtoBuilder = new CreateTransactionDtoBuilder();
        }

        protected TransactionEntity GetBalance(CreateTransactionDto transaction)
        {
            return fixture
                .GetData<TransactionEntity>()
                .First(
                    tra =>
                        tra.BalanceId      == transaction.BalanceId &&
                        tra.CategoryId     == transaction.CategoryId &&
                        tra.TargetDate     == transaction.TargetDate &&
                        tra.FinishDate     == transaction.FinishDate &&
                        tra.Amount         == transaction.Amount &&
                        tra.Type           == transaction.Type &&
                        tra.Description    == transaction.Description &&
                        tra.IsActive       == transaction.IsActive 
                );
        }

        [Test]
        public async Task Post_ValidTransaction_ReturnCreatedTransaction()
        {
            var entity = transactionBuilder.Generate();

            fixture.AddData(entity.Category);
            fixture.AddData(entity.Balance);

            var data = createTransactionDtoBuilder
                .WithCategoryId(entity.Category.Id)
                .WithBalanceId(entity.Balance.Id)
                .Generate();

            var response = await client.PostAsync(baseEndpoint, data);
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
        public async Task Post_ValidTransaction_CreateTransaction()
        {
            var entity = transactionBuilder.Generate();

            fixture.AddData(entity.Category);
            fixture.AddData(entity.Balance);

            var body = createTransactionDtoBuilder
                .WithCategoryId(entity.Category.Id)
                .WithBalanceId(entity.Balance.Id)
                .Generate();

            await client.PostAsync(baseEndpoint, body);

            var balance = GetBalance(body);
            Assert.IsNotNull(balance);
        }
    }
}
