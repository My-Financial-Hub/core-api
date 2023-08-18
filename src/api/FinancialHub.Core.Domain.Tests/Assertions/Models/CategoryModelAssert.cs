namespace FinancialHub.Core.Domain.Tests.Assertions.Models
{
    public static class CategoryModelAssert
    {
        public static void Equal(CategoryModel expected, CategoryModel result)
        {
            Assert.AreEqual(expected.Name, result.Name);
            Assert.AreEqual(expected.Description, result.Description);
            Assert.AreEqual(expected.IsActive, result.IsActive);
        }
    }
}
