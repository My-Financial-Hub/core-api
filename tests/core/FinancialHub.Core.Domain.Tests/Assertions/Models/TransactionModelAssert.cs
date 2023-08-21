namespace FinancialHub.Core.Domain.Tests.Assertions.Models
{
    public static class TransactionModelAssert
    {
        public static void Equal(TransactionModel expected, TransactionModel result)
        {
            Assert.AreEqual(expected.BalanceId, result.BalanceId);
            Assert.AreEqual(expected.CategoryId, result.CategoryId);
            Assert.AreEqual(expected.TargetDate, result.TargetDate);
            Assert.AreEqual(expected.FinishDate, result.FinishDate);
            Assert.AreEqual(expected.Amount, result.Amount);
            Assert.AreEqual(expected.Type, result.Type);
            Assert.AreEqual(expected.Description, result.Description);
            Assert.AreEqual(expected.IsActive, result.IsActive);
        }
    }
}
