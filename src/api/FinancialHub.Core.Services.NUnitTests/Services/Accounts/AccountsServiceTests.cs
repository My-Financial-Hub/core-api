using AutoMapper;
using FinancialHub.Core.Domain.Interfaces.Mappers;
using FinancialHub.Core.Domain.Interfaces.Repositories;
using FinancialHub.Core.Domain.Interfaces.Services;
using FinancialHub.Core.Domain.Tests.Builders.Entities;
using FinancialHub.Core.Services.Mappers;
using FinancialHub.Core.Services.Services;

namespace FinancialHub.Core.Services.NUnitTests.Services
{
    public partial class AccountsServiceTests
    {
        protected Random random;
        protected AccountEntityBuilder accountEntityBuilder; 
        protected AccountModelBuilder accountModelBuilder; 
        
        private IAccountsService service;

        private IMapper mapper;
        private Mock<IMapperWrapper> mapperWrapper;
        private Mock<IAccountsRepository> repository;

        private void MockMapper()
        {
            mapper = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new FinancialHubAutoMapperProfile());
                }
            ).CreateMapper();

            this.mapperWrapper = new Mock<IMapperWrapper>();
        }

        [SetUp]
        public void Setup()
        {
            this.MockMapper();

            this.repository = new Mock<IAccountsRepository>();
            this.service = new AccountsService(mapperWrapper.Object,repository.Object);

            this.random = new Random();

            this.accountEntityBuilder = new AccountEntityBuilder();
            this.accountModelBuilder = new AccountModelBuilder();
        }

        public ICollection<AccountEntity> GenerateAccounts()
        {
            return this.accountEntityBuilder.Generate(random.Next(5,10));
        }
    }
}
