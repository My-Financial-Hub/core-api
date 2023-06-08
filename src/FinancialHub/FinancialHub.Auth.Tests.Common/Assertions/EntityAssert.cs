﻿using FinancialHub.Auth.Domain.Entities;
using FinancialHub.Auth.Domain.Models;

namespace FinancialHub.Auth.Tests.Common.Assertions
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
    }
}
