namespace FinancialHub.Core.Domain.Tests.Assertions.Entities
{
    public static class BalanceEntityAssert
    {
        public static void Equal(BalanceEntity expected, BalanceEntity result)
        {
            Assert.AreEqual(expected.Name, result.Name);
            Assert.AreEqual(expected.AccountId, result.AccountId);
            Assert.AreEqual(expected.IsActive, result.IsActive);
        }

        public static void Equal(BalanceEntity expected, BalanceModel result)
        {
            Assert.AreEqual(expected.Name, result.Name);
            Assert.AreEqual(expected.AccountId, result.AccountId);
            Assert.AreEqual(expected.IsActive, result.IsActive);
        }
    }
}
