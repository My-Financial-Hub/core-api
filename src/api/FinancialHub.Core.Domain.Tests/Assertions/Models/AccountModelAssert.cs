namespace FinancialHub.Core.Domain.Tests.Assertions.Models
{
    public static class AccountModelAssert
    {
        public static void Equal(AccountModel expected, AccountModel result)
        {
            Assert.AreEqual(expected.Name, result.Name);
            Assert.AreEqual(expected.Description, result.Description);
            Assert.AreEqual(expected.IsActive, result.IsActive);
        }
    }
}
