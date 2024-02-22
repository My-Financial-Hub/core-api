using AutoMapper;
using FinancialHub.Core.Application.Mappers;
using FinancialHub.Core.Application.Services;
using FinancialHub.Core.Domain.Interfaces.Resources;

namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class AccountsServiceTests
    {
        protected Random random;
        protected AccountModelBuilder accountModelBuilder; 
        
        private IAccountsService service;

        private Mock<IAccountsProvider> provider;
        private Mock<IErrorMessageProvider> errorMessageProvider;

        private IMapper mapper;

        private void MockMapper()
        {
            mapper = new MapperConfiguration(
                mc =>
                {
                    mc.AddProfile(new AccountMapper());
                }
            ).CreateMapper();
        }

        [SetUp]
        public void Setup()
        {
            this.provider = new Mock<IAccountsProvider>();
            this.errorMessageProvider = new Mock<IErrorMessageProvider>();
            this.MockMapper();
            this.service = new AccountsService(provider.Object, mapper, errorMessageProvider.Object);

            this.random = new Random();

            this.accountModelBuilder = new AccountModelBuilder();
            this.AddCreateAccountBuilder();
        }
    }
}
