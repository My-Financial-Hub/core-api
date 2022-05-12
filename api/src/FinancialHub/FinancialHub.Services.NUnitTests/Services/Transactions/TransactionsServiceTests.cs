using AutoMapper;
using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Interfaces.Mappers;
using FinancialHub.Domain.Interfaces.Repositories;
using FinancialHub.Domain.Interfaces.Services;
using FinancialHub.Domain.Models;
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
        private Mock<IAccountsRepository> accountsRepository;
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
            this.accountsRepository = new Mock<IAccountsRepository>();
            this.categoriesRepository = new Mock<ICategoriesRepository>();
            this.service = new TransactionsService(mapperWrapper.Object,repository.Object,accountsRepository.Object,categoriesRepository.Object);

            this.random = new Random();

            this.transactionBuilder = new TransactionEntityBuilder();
            this.transactionModelBuilder = new TransactionModelBuilder();
        }

        private void SetUpMapper<T,Y>()
        {
            this.mapperWrapper
                .Setup(x => x.Map<Y>(It.IsAny<T>()))
                .Returns<T>((ent) => this.mapper.Map<Y>(ent))
                .Verifiable();

            this.mapperWrapper
                .Setup(x => x.Map<T>(It.IsAny<Y>()))
                .Returns<Y>((model) => this.mapper.Map<T>(model))
                .Verifiable();
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
