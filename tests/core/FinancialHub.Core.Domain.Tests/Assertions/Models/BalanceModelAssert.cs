namespace FinancialHub.Core.Domain.Tests.Assertions.Models
{
    public static class BalanceModelAssert
    {
        public static void Equal(BalanceModel expected, BalanceModel result)
        {
            Assert.AreEqual(expected.Name, result.Name);
            Assert.AreEqual(expected.AccountId, result.AccountId);
            Assert.AreEqual(expected.IsActive, result.IsActive);
        }

        public static void Equal(BalanceModel[] expected, BalanceModel[]? result)
        {
            Assert.That(result, Is.Not.Null);

            for (int i = 0; i < expected.Length; i++)
            {
                Equal(expected[i], result![i]);
            }
        }

        public static void Equal(BalanceModel expected, BalanceEntity result)
        {
            Assert.AreEqual(expected.Name, result.Name);
            Assert.AreEqual(expected.AccountId, result.AccountId);
            Assert.AreEqual(expected.IsActive, result.IsActive);
        }
    }
}
