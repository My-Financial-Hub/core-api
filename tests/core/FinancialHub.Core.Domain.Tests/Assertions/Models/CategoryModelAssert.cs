namespace FinancialHub.Core.Domain.Tests.Assertions.Models
{
    public static class CategoryModelAssert
    {
        public static void Equal(CategoryModel expected, CategoryModel? result)
        {
            Assert.That(result, Is.Not.Null);
            Assert.AreEqual(expected.Name, result!.Name);
            Assert.AreEqual(expected.Description, result.Description);
            Assert.AreEqual(expected.IsActive, result.IsActive);
        }

        public static void Equal(CategoryModel[] expected, CategoryModel[]? result)
        {
            Assert.That(result, Is.Not.Null);

            for (int i = 0; i < expected.Length; i++)
            {
                Equal(expected[i], result![i]);
            }
        }
    }
}
