using AutoMapper;
using FinancialHub.Domain.Entities;
using FinancialHub.Domain.Interfaces.Mappers;
using FinancialHub.Domain.Interfaces.Repositories;
using FinancialHub.Domain.Interfaces.Services;
using FinancialHub.Domain.NUnitTests.Generators;
using FinancialHub.Domain.Tests.Builders.Entities;
using FinancialHub.Services.Mappers;
using FinancialHub.Services.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace FinancialHub.Services.NUnitTests.Services
{
    public partial class AccountsServiceTests
    {
        protected Random random;
        protected AccountEntityBuilder accountEntityBuilder; 
        protected ModelGenerator modelGenerator; 
        
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
            this.modelGenerator = new ModelGenerator(random);
        }

        public ICollection<AccountEntity> GenerateAccounts(){
            return this.accountEntityBuilder.Generate(random.Next(5,10));
        }
    }
}
