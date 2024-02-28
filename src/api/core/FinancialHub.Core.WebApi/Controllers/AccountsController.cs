using FinancialHub.Core.Domain.DTOS.Accounts;
using FinancialHub.Core.Domain.DTOS.Balances;
using Microsoft.Extensions.Logging;

namespace FinancialHub.Core.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [ProducesErrorResponseType(typeof(Exception))]
    public class AccountsController : BaseController
    {
        private readonly IAccountsService service;
        private readonly IBalancesService balanceService;
        private readonly ILogger<AccountsController> logger;

        public AccountsController(IAccountsService service, IBalancesService balanceService, ILogger<AccountsController> logger) 
        {
            this.service = service;
            this.balanceService = balanceService;
            this.logger = logger;
        }

        /// <summary>
        /// Get all balances that belongs to an account 
        /// </summary>
        /// <param name="accountId">id of the account</param>
        [HttpGet("{accountId}/balances")]
        [ProducesResponseType(typeof(ListResponse<BalanceDto>), 200)]
        [ProducesResponseType(typeof(NotFoundErrorResponse), 404)]
        public async Task<IActionResult> GetAccountBalances([FromRoute] Guid accountId)
        {
            var result = await this.balanceService.GetAllByAccountAsync(accountId);

            if (result.HasError)
            {
                this.logger.LogWarning(
                    "Error getting balances from account {accountId} : {Message}",
                    accountId, result.Error.Message
                );
                return StatusCode(
                    result.Error.Code,
                    new NotFoundErrorResponse(result.Error.Message)
                 );
            }

            return Ok(new ListResponse<BalanceDto>(result.Data));
        }

        /// <summary>
        /// Get all accounts of the system 
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ListResponse<AccountDto>), 200)]
        public async Task<IActionResult> GetMyAccounts()
        {
            var result = await service.GetAllAsync();

            return Ok(new ListResponse<AccountDto>(result.Data));
        }

        /// <summary>
        /// Creates an account on database 
        /// </summary>
        /// <param name="account">Account to be created</param>
        [HttpPost]
        [ProducesResponseType(typeof(SaveResponse<AccountDto>), 200)]
        [ProducesResponseType(typeof(ValidationsErrorResponse), 400)]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto account)
        {
            var result = await this.service.CreateAsync(account);

            if (result.HasError)
            {
                this.logger.LogWarning(
                    "Error creating account : {Message}",
                    result.Error.Message
                );
                return ErrorResponse(result.Error);
            }

            return SaveResponse(result.Data);
        }

        /// <summary>
        /// Updates an existing account on database
        /// </summary>
        /// <param name="id">id of the account</param>
        /// <param name="account">account changes</param>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SaveResponse<AccountDto>), 200)]
        [ProducesResponseType(typeof(NotFoundErrorResponse), 404)]
        [ProducesResponseType(typeof(ValidationsErrorResponse), 400)]
        public async Task<IActionResult> UpdateAccount([FromRoute] Guid id, [FromBody] UpdateAccountDto account)
        {
            var result = await service.UpdateAsync(id, account);

            if (result.HasError)
            {
                this.logger.LogWarning(
                    "Error updating account {id} : {Message}",
                    id, result.Error.Message
                );
                return ErrorResponse(response.Error);
            }

            return SaveResponse(response.Data);
        }

        /// <summary>
        /// Deletes an existing account on database
        /// </summary>
        /// <param name="id">id of the account</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteAccount([FromRoute] Guid id)
        {
            await service.DeleteAsync(id);
            return NoContent();
        }
    }
}
