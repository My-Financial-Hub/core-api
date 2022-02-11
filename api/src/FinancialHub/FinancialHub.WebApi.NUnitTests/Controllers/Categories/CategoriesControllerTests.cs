using Moq;
using System;
using NUnit.Framework;
using FinancialHub.WebApi.Controllers;
using FinancialHub.Domain.Interfaces.Services;
using FinancialHub.Domain.NUnitTests.Generators;

namespace FinancialHub.WebApi.NUnitTests.Controllers
{
    public partial class CategoriesControllerTests
    {
        private Random random;
        private ModelGenerator modelGenerator;

        private CategoriesController controller;
        private Mock<ICategoriesService> mockService;

        [SetUp]
        public void Setup()
        {
            this.random = new Random();
            this.modelGenerator = new ModelGenerator(random);

            this.mockService = new Mock<ICategoriesService>();
            this.controller = new CategoriesController(this.mockService.Object);
        }
    }
}