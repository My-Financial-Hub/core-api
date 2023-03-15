using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FinancialHub.Domain.Interfaces.Services;
using FinancialHub.Domain.Models;
using FinancialHub.Domain.Responses.Success;
using FinancialHub.Domain.Responses.Errors;

namespace FinancialHub.WebApi.Controllers
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
        [ProducesResponseType(typeof(SaveResponse<BalanceModel>), 200)]
        [ProducesResponseType(typeof(ValidationErrorResponse), 400)]
        public async Task<IActionResult> CreateBalance([FromBody] BalanceModel balance)
        {
            var result = await this.service.CreateAsync(balance);

            if (result.HasError)
            {
                return StatusCode(
                    result.Error.Code,
                    new ValidationErrorResponse(result.Error.Message)
                 );
            }

            return Ok(new SaveResponse<BalanceModel>(result.Data));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SaveResponse<AccountModel>), 200)]
        [ProducesResponseType(typeof(NotFoundErrorResponse), 404)]
        [ProducesResponseType(typeof(ValidationErrorResponse), 400)]
        public async Task<IActionResult> UpdateBalance([FromRoute] Guid id, [FromBody] BalanceModel balance)
        {
            var response = await this.service.UpdateAsync(id, balance);

            if (response.HasError)
            {
                return StatusCode(
                    response.Error.Code,
                    new ValidationErrorResponse(response.Error.Message)
                 );
            }

            return Ok(new SaveResponse<BalanceModel>(response.Data));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBalance([FromRoute] Guid id)
        {
            await this.service.DeleteAsync(id);
            return NoContent();
        }
    }
}
