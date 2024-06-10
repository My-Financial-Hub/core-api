using FinancialHub.Core.Application.Services;
using AutoMapper;
using FinancialHub.Core.Application.Mappers;
using FinancialHub.Core.Domain.Interfaces.Validators;

namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class BalancesServiceTests
    {
        protected Random random;
        protected BalanceModelBuilder balanceModelBuilder;

        private IBalancesService service;

        private Mock<IBalancesProvider> provider; 
        private Mock<IBalancesValidator> validator;
        private Mock<IAccountsValidator> accountValidator;
        private Mock<ILogger<BalancesService>> mockLogger;

        private IMapper mapper;

        private void MockMapper()
        {
            mapper = new MapperConfiguration(
                mc =>
                {
                    mc.AddProfile(new BalanceMapper());
                }
            ).CreateMapper();
        }

        [SetUp]
        public void Setup()
        {
            this.provider           = new Mock<IBalancesProvider>();
            this.validator          = new Mock<IBalancesValidator>();
            this.accountValidator   = new Mock<IAccountsValidator>();
            this.MockMapper();
            this.service            = new BalancesService(
                provider.Object,
                validator.Object,
                accountValidator.Object,
                mapper,
                new NullLoggerFactory().CreateLogger<BalancesService>()
            );

            this.random = new Random();

            this.balanceModelBuilder = new BalanceModelBuilder();
            this.AddCreateBalanceBuilder();
            this.AddUpdateBalanceBuilder();
        }
    }
}
