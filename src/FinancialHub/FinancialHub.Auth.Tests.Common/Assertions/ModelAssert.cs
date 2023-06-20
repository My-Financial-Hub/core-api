namespace FinancialHub.Auth.Tests.Common.Assertions
{
    public static class ModelAssert
    {
        public static void Equal(UserModel expected, UserModel actual)
        {
            Assert.Multiple(() =>
            {
                Assert.That(actual.Id, Is.EqualTo(expected.Id));
                Assert.That(actual.FirstName, Is.EqualTo(expected.FirstName));
                Assert.That(actual.LastName, Is.EqualTo(expected.LastName));
                Assert.That(actual.Email, Is.EqualTo(expected.Email));
                Assert.That(actual.BirthDate, Is.EqualTo(expected.BirthDate));
            });
        }
    }
}
