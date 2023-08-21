using NUnit.Framework;

namespace FinancialHub.Auth.Common.Tests.Assertions
{
    public static class EntityAssert
    {
        public static void Equal(UserEntity expected, UserEntity result)
        {
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(expected.Id));
                Assert.That(result.FirstName, Is.EqualTo(expected.FirstName));
                Assert.That(result.LastName, Is.EqualTo(expected.LastName));
                Assert.That(result.Email, Is.EqualTo(expected.Email));
                Assert.That(result.BirthDate, Is.EqualTo(expected.BirthDate));
            });
        }

        public static void Equal(UserEntity expected, UserModel result)
        {
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(expected.Id));
                Assert.That(result.FirstName, Is.EqualTo(expected.FirstName));
                Assert.That(result.LastName, Is.EqualTo(expected.LastName));
                Assert.That(result.Email, Is.EqualTo(expected.Email));
                Assert.That(result.BirthDate, Is.EqualTo(expected.BirthDate));
            });
        }

        public static void Equal(CredentialEntity expected, CredentialEntity result)
        {
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(expected.Id));
                Assert.That(result.Password, Is.EqualTo(expected.Password));
                Assert.That(result.Login, Is.EqualTo(expected.Login));
                Assert.That(result.UserId, Is.EqualTo(expected.UserId));
            });
        }
    }
}
