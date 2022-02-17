using AutoMapper;
using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Interfaces.Mappers;
using FinancialHub.Domain.Interfaces.Repositories;
using FinancialHub.Domain.Interfaces.Services;
using FinancialHub.Domain.Tests.Builders.Entities;
using FinancialHub.Domain.Tests.Builders.Models;
using FinancialHub.Services.Mappers;
using FinancialHub.Services.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace FinancialHub.Services.NUnitTests.Services
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

            this.transactionBuilder = new TransactionEntityBuilder();
            this.transactionModelBuilder = new TransactionModelBuilder();
        }

        public ICollection<TransactionEntity> GenerateTransactions()
        {
            return this.transactionBuilder.Generate(random.Next(5, 10));
        }
    }
}
