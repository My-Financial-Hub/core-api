using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FinancialHub.Domain.Interfaces.Services;
using FinancialHub.Domain.Models;
using FinancialHub.Domain.Responses.Success;

namespace FinancialHub.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [ProducesErrorResponseType(typeof(Exception))]
    public class BalanceController : Controller
    {
        private readonly IBalancesService service;

        public BalanceController(IBalancesService service)
        {
            this.service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ListResponse<BalanceModel>), 200)]
        public async Task<IActionResult> GetBalances([FromRoute] Guid accountId)
        {
            var result = await service.GetAllByAccountAsync(accountId);

            return Ok(new ListResponse<BalanceModel>(result.Data));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount([FromRoute] Guid id)
        {
            await this.service.DeleteAsync(id);
            return NoContent();
        }
    }
}
