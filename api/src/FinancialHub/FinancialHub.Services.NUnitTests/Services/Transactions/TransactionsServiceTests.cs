using AutoMapper;
using FinancialHub.Domain.Interfaces.Mappers;
using FinancialHub.Domain.Interfaces.Repositories;
using FinancialHub.Domain.Interfaces.Services;
using FinancialHub.Domain.NUnitTests.Generators;
using FinancialHub.Services.Mappers;
using FinancialHub.Services.Services;
using Moq;
using NUnit.Framework;
using System;

namespace FinancialHub.Services.NUnitTests.Services
{
    public partial class TransactionsServiceTests
    {
        protected Random random;
        protected EntityGenerator entityGenerator; 
        protected ModelGenerator modelGenerator; 
        
        private ITransactionsService service;

        private IMapper mapper;
        private Mock<IMapperWrapper> mapperWrapper;
        private Mock<ITransactionsRepository> repository;

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
            this.service = new TransactionsService(mapperWrapper.Object,repository.Object);

            this.random = new Random();

            this.entityGenerator = new EntityGenerator(random);
            this.modelGenerator = new ModelGenerator(random);
        }
    }
}
