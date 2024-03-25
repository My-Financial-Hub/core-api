using FinancialHub.Core.WebApi.Controllers;
using FinancialHub.Core.Domain.Interfaces.Services;
using FinancialHub.Core.Domain.Tests.Builders.DTOS.Balances;
using Microsoft.Extensions.Logging;

namespace FinancialHub.Core.WebApi.Tests.Controllers
{
    public partial class BalancesControllerTests
    {
        private BalanceDtoBuilder balanceDtoBuilder;
        private CreateBalanceDtoBuilder createBalanceDtoBuilder;
        private UpdateBalanceDtoBuilder updateBalanceDtoBuilder;

        private BalancesController controller;
        private Mock<IBalancesService> mockService;
        private Mock<ILogger<BalancesController>> mockLogger;

        [SetUp]
        public void Setup()
        {
            this.balanceDtoBuilder = new BalanceDtoBuilder();
            this.createBalanceDtoBuilder = new CreateBalanceDtoBuilder();
            this.updateBalanceDtoBuilder = new UpdateBalanceDtoBuilder();

            this.mockService = new Mock<IBalancesService>();
            this.mockLogger = new Mock<ILogger<BalancesController>>();
            this.controller = new BalancesController(this.mockService.Object, this.mockLogger.Object);
        }
    }
}
