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
    public partial class CategoriesServiceTests
    {
        protected Random random;
        protected CategoryEntityBuilder categoryBuilder; 
        protected CategoryModelBuilder categoryModelBuilder; 
        
        private ICategoriesService service;

        private IMapper mapper;
        private Mock<IMapperWrapper> mapperWrapper;
        private Mock<ICategoriesRepository> repository;

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

            this.repository = new Mock<ICategoriesRepository>();
            this.service = new CategoriesService(mapperWrapper.Object,repository.Object);

            this.random = new Random();

            this.categoryBuilder = new CategoryEntityBuilder();
            this.categoryModelBuilder = new CategoryModelBuilder();
        }

        private ICollection<CategoryEntity> CreateCategories()
        {
            return this.categoryBuilder.Generate(random.Next(10, 100));
        }
    }
}
