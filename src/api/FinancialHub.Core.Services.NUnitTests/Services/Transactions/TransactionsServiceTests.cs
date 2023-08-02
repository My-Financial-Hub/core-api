using AutoMapper;
using FinancialHub.Core.Domain.Interfaces.Mappers;
using FinancialHub.Core.Domain.Interfaces.Repositories;
using FinancialHub.Core.Domain.Interfaces.Services;
using FinancialHub.Core.Domain.Tests.Builders.Entities;
using FinancialHub.Core.Services.Mappers;
using FinancialHub.Core.Services.Services;

namespace FinancialHub.Core.Services.NUnitTests.Services
{
    public partial class TransactionsServiceTests
    {
        protected Random random;
        protected TransactionEntityBuilder transactionBuilder; 
        protected TransactionModelBuilder transactionModelBuilder; 
        
        private ITransactionsService service;

        private IMapper mapper;
        private Mock<IMapperWrapper> mapperWrapper;
        private Mock<ITransactionsRepository> repository;
        private Mock<IBalancesRepository> balancesRepository;
        private Mock<ICategoriesRepository> categoriesRepository;

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

            this.repository = new Mock<ITransactionsRepository>();
            this.balancesRepository = new Mock<IBalancesRepository>();
            this.categoriesRepository = new Mock<ICategoriesRepository>();
            this.service = new TransactionsService(mapperWrapper.Object,repository.Object,balancesRepository.Object,categoriesRepository.Object);

            this.random = new Random();

            this.transactionBuilder = new TransactionEntityBuilder();
            this.transactionModelBuilder = new TransactionModelBuilder();
        }

        private void SetUpMapper()
        {
            this.mapperWrapper
                .Setup(x => x.Map<TransactionModel>(It.IsAny<TransactionEntity>()))
                .Returns<TransactionEntity>((ent) => this.mapper.Map<TransactionModel>(ent))
                .Verifiable();

            this.mapperWrapper
                .Setup(x => x.Map<TransactionEntity>(It.IsAny<TransactionModel>()))
                .Returns<TransactionModel>((model) => this.mapper.Map<TransactionEntity>(model))
                .Verifiable();
        }

        public ICollection<TransactionEntity> GenerateTransactions()
        {
            return this.transactionBuilder.Generate(random.Next(5, 10));
        }
    }
}
