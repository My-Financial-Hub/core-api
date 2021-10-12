using System;
using System.Threading.Tasks;
using FinancialHub.Infra.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FinancialHub.WebApi.Controllers
{
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class AccountsController : Controller
    {
        private readonly ILogger logger;
        private readonly FinancialHubContext dbContext;
        public AccountsController(FinancialHubContext dbContext/*,ILogger logger*/)
        {
            //this.logger = logger;
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Basic endpoint for tests
        /// </summary>
        /// <returns>Returns the current date time</returns>
        [HttpGet]
        [Route("metadata")]
        [ProducesDefaultResponseType(typeof(DateTimeOffset))]
        [ProducesErrorResponseType(typeof(Exception))]
        [Obsolete("Will be removed in the next commit")]
        public async Task<IActionResult> GetMetadataAsync()
        {
            return Ok(DateTimeOffset.Now);
        }
    }
}
