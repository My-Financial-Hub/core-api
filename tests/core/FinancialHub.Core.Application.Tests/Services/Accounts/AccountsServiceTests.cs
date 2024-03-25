using AutoMapper;
using FinancialHub.Core.Application.Mappers;
using FinancialHub.Core.Application.Services;
using FinancialHub.Core.Domain.Interfaces.Resources;
using FinancialHub.Core.Domain.Interfaces.Validators;
using Microsoft.Extensions.Logging;

namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class AccountsServiceTests
    {
        protected Random random;
        protected AccountModelBuilder accountModelBuilder; 
        
        private IAccountsService service;

        private Mock<IAccountsProvider> provider;
        private Mock<IAccountsValidator> validator;
        private Mock<ILogger<AccountsService>> mockLogger;

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
            this.MockMapper();
            this.validator = new Mock<IAccountsValidator>();
            this.mockLogger = new Mock<ILogger<AccountsService>>();

            this.service = new AccountsService(
                provider.Object,
                validator.Object,
                mapper,
                mockLogger.Object
            );

            this.random = new Random();

            this.accountModelBuilder = new AccountModelBuilder();
            this.AddCreateAccountBuilder();
            this.AddUpdateAccountBuilder();
        }
    }
}
