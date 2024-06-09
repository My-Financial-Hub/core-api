using AutoMapper;
using FinancialHub.Core.Application.Mappers;
using FinancialHub.Core.Application.Services;
using FinancialHub.Core.Domain.Interfaces.Resources;
using FinancialHub.Core.Domain.Interfaces.Validators;
using Microsoft.Extensions.Logging;

namespace FinancialHub.Core.Application.Tests.Services
{
    public partial class TransactionsServiceTests
    {
        protected Random random;
        protected TransactionModelBuilder transactionModelBuilder; 
        
        private ITransactionsService service;

        private Mock<ITransactionsProvider> provider;
        private Mock<ITransactionsValidator> validator;
        private Mock<IBalancesValidator> balancesValidator;
        private Mock<ICategoriesValidator> categoriesValidator;

        private IMapper mapper;

        private void MockMapper()
        {
            mapper = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new TransactionMapper());
                }
            ).CreateMapper();
        }

        [SetUp]
        public void Setup()
        {
            this.provider               = new Mock<ITransactionsProvider>();
            this.validator              = new Mock<ITransactionsValidator>();
            this.balancesValidator      = new Mock<IBalancesValidator>();
            this.categoriesValidator    = new Mock<ICategoriesValidator>();
            this.MockMapper();
            this.service = new TransactionsService(
                provider.Object,
                validator.Object,
                balancesValidator.Object,
                categoriesValidator.Object,
                mapper,
                new NullLoggerFactory().CreateLogger<TransactionsService>()
            );

            this.random = new Random();

            this.transactionModelBuilder = new TransactionModelBuilder();
            this.AddCreateTransactionBuilder();
            this.AddUpdateTransactionBuilder();
        }
    }
}
