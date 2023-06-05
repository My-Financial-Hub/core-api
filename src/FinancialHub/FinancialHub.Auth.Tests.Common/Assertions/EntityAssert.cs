using NUnit.Framework;
using FinancialHub.Auth.Domain.Entities;

namespace FinancialHub.Auth.Tests.Common.Assertions
{
    public static class EntityAssert
    {
        public static void Equal(UserEntity expected, UserEntity result)
        {
            Assert.Multiple(() =>
            {
                Assert.That(result.FirstName, Is.EqualTo(expected.FirstName));
                Assert.That(result.LastName, Is.EqualTo(expected.LastName));
                Assert.That(result.Email, Is.EqualTo(expected.Email));
                Assert.That(result.BirthDate, Is.EqualTo(expected.BirthDate));
            });
        }
    }
}
