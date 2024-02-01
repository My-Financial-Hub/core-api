using FinancialHub.Core.Domain.DTOS.Balances;

namespace FinancialHub.Core.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [ProducesErrorResponseType(typeof(Exception))]
    public class BalancesController : Controller
    {
        private readonly IBalancesService service;

        public BalancesController(IBalancesService service)
        {
            this.service = service;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SaveResponse<BalanceDto>), 200)]
        [ProducesResponseType(typeof(ValidationErrorResponse), 400)]
        public async Task<IActionResult> CreateBalance([FromBody] CreateBalanceDto balance)
        {
            var result = await this.service.CreateAsync(balance);

            if (result.HasError)
            {
                return StatusCode(
                    result.Error.Code,
                    new ValidationErrorResponse(result.Error.Message)
                 );
            }

            return Ok(new SaveResponse<BalanceDto>(result.Data));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SaveResponse<BalanceDto>), 200)]
        [ProducesResponseType(typeof(NotFoundErrorResponse), 404)]
        [ProducesResponseType(typeof(ValidationErrorResponse), 400)]
        public async Task<IActionResult> UpdateBalance([FromRoute] Guid id, [FromBody] UpdateBalanceDto balance)
        {
            var response = await this.service.UpdateAsync(id, balance);

            if (response.HasError)
            {
                return StatusCode(
                    response.Error.Code,
                    new ValidationErrorResponse(response.Error.Message)
                 );
            }

            return Ok(new SaveResponse<BalanceDto>(response.Data));
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
