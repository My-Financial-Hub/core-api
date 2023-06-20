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

        public static void Equal(CredentialModel expected, CredentialEntity result)
        {
            Assert.Multiple(() =>
            {
                Assert.That(result.Password, Is.EqualTo(expected.Password));
                Assert.That(result.Login, Is.EqualTo(expected.Login));
                Assert.That(result.UserId, Is.EqualTo(expected.UserId));
            });
        }
    }
}
