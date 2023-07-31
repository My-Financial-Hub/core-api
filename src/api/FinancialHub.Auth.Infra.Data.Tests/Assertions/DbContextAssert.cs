namespace FinancialHub.Auth.Infra.Data.Tests.Assertions
{
    internal static class DbContextAssert
    {
        internal static void AssertCreated<T>(DbContext context,T createdItem) 
            where T : BaseEntity
        {
            Assert.Multiple(() =>
            {
                Assert.That(context.Set<T>().ToList(), Is.Not.Empty);

                var datebaseUser = context.Set<T>().First(u => u.Id == createdItem.Id);
                Assert.That(datebaseUser, Is.EqualTo(createdItem));
            });
        }
    }
}
