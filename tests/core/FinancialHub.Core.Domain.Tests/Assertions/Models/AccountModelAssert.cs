namespace FinancialHub.Core.Domain.Tests.Assertions.Models
{
    public static class AccountModelAssert
    {
        public static void Equal(AccountModel expected, AccountModel? result)
        {
            Assert.That(result, Is.Not.Null);
            Assert.AreEqual(expected.Name, result!.Name);
            Assert.AreEqual(expected.Description, result.Description);
            Assert.AreEqual(expected.IsActive, result.IsActive);
        }

        public static void Equal(AccountModel[] expected, AccountModel[]? result)
        {
            Assert.That(result, Is.Not.Null);

            for (int i = 0; i < expected.Length; i++)
            {
                Equal(expected[i], result![i]);
            }
        }
    }
}
