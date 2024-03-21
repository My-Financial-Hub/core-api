using FinancialHub.Core.Domain.DTOS.Balances;
using Microsoft.Extensions.Logging;

namespace FinancialHub.Core.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [ProducesErrorResponseType(typeof(Exception))]
    public class BalancesController : BaseController
    {
        private readonly IBalancesService service;
        private readonly ILogger<BalancesController> logger;

        public BalancesController(IBalancesService service, ILogger<BalancesController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SaveResponse<BalanceDto>), 200)]
        [ProducesResponseType(typeof(ValidationsErrorResponse), 400)]
        public async Task<IActionResult> CreateBalance([FromBody] CreateBalanceDto balance)
        {
            this.logger.LogInformation("Starting creation of balance");
            var result = await this.service.CreateAsync(balance);

            if (result.HasError)
            {
                this.logger.LogWarning(
                    "Error creating balance : {Message}",
                    result.Error.Message
                );
                return ErrorResponse(result.Error);
            }

            this.logger.LogInformation("Finished creation of Balance");
            return SaveResponse(result.Data);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SaveResponse<BalanceDto>), 200)]
        [ProducesResponseType(typeof(NotFoundErrorResponse), 404)]
        [ProducesResponseType(typeof(ValidationsErrorResponse), 400)]
        public async Task<IActionResult> UpdateBalance([FromRoute] Guid id, [FromBody] UpdateBalanceDto balance)
        {
            var result = await this.service.UpdateAsync(id, balance);

            if (result.HasError)
            {
                return ErrorResponse(result.Error);
            }

            return SaveResponse(result.Data);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteBalance([FromRoute] Guid id)
        {
            this.logger.LogInformation("Removing balance");
            await service.DeleteAsync(id);
            this.logger.LogInformation("Balance removed");

            return NoContent();
        }
    }
}
