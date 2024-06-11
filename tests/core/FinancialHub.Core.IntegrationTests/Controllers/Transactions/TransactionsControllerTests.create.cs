using FinancialHub.Core.Domain.DTOS.Transactions;
using FinancialHub.Core.Domain.Enums;
using FinancialHub.Core.Domain.Tests.Builders.DTOS.Transactions;

namespace FinancialHub.Core.IntegrationTests.Controllers
{
    public partial class TransactionsControllerTests
    {
        private CreateTransactionDtoBuilder createTransactionDtoBuilder;
        protected void AddCreateTransactionBuilder()
        {
            createTransactionDtoBuilder = new CreateTransactionDtoBuilder();
        }

        protected TransactionEntity GetTransaction(CreateTransactionDto transaction)
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
        public async Task Post_InvalidTransaction_Returns400BadRequest()
        {
            var entity = transactionBuilder.Generate();

            fixture.AddData(entity.Category);
            fixture.AddData(entity.Balance);

            var body = createTransactionDtoBuilder
                .WithAmount(-1)
                .WithDescription(new string('o',501))
                .WithCategoryId(entity.Category.Id)
                .WithBalanceId(entity.Balance.Id)
                .WithType((TransactionType)999)
                .WithStatus((TransactionStatus)999)
                .Generate();

            var response = await client.PostAsync(baseEndpoint, body);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task Post_InvalidTransaction_ReturnsTransactionValidationError()
        {
            var entity = transactionBuilder.Generate();

            fixture.AddData(entity.Category);
            fixture.AddData(entity.Balance);

            var body = createTransactionDtoBuilder
                .WithAmount(-1)
                .WithDescription(new string('o', 501))
                .WithCategoryId(entity.Category.Id)
                .WithBalanceId(entity.Balance.Id)
                .WithType((TransactionType)999)
                .WithStatus((TransactionStatus)999)
                .Generate();

            var response = await client.PostAsync(baseEndpoint, body);
            var validationResponse = await response.ReadContentAsync<ValidationsErrorResponse>();

            Assert.IsNotNull(validationResponse);
            Assert.AreEqual(4, validationResponse!.Errors.Length);
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

            var balance = GetTransaction(body);
            Assert.IsNotNull(balance);
        }
    }
}
