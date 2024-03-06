using FinancialHub.Core.Domain.DTOS.Balances;

namespace FinancialHub.Core.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [ProducesErrorResponseType(typeof(Exception))]
    public class BalancesController : BaseController
    {
        private readonly IBalancesService service;

        public BalancesController(IBalancesService service)
        {
            this.service = service;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SaveResponse<BalanceDto>), 200)]
        [ProducesResponseType(typeof(ValidationsErrorResponse), 400)]
        public async Task<IActionResult> CreateBalance([FromBody] CreateBalanceDto balance)
        {
            var result = await this.service.CreateAsync(balance);

            if (result.HasError)
            {
                return ErrorResponse(result.Error);
            }

            return Ok(new SaveResponse<BalanceDto>(result.Data));
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

            return Ok(new SaveResponse<BalanceDto>(result.Data));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteBalance([FromRoute] Guid id)
        {
            await this.service.DeleteAsync(id);
            return NoContent();
        }
    }
}
