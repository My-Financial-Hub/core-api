using AutoMapper;
using FinancialHub.Auth.Domain.Interfaces.Providers;
using FinancialHub.Auth.Infra.Providers;
using FinancialHub.Auth.Domain.Interfaces.Repositories;
using FinancialHub.Auth.Tests.Common.Builders.Entities;
using FinancialHub.Auth.Infra.Mappers;
using FinancialHub.Auth.Domain.Models;

namespace FinancialHub.Auth.Infra.Tests.Providers
{
    public partial class UserProviderTests
    {
        private IMapper mapper;
        private Mock<IUserRepository> mockRepository;
        private IUserProvider provider;

        private UserModelBuilder builder;

        [SetUp]
        public void SetUp() 
        {
            this.builder = new UserModelBuilder();

            this.mapper = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new FinancialHubAuthProviderProfile());
            }
            ).CreateMapper();
            this.mockRepository = new Mock<IUserRepository>();
            this.provider = new UserProvider(this.mockRepository.Object, this.mapper);
        }

        protected static void AssertEqual(UserModel expected,UserModel actual)
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
