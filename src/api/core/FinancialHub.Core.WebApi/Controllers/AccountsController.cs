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
            this.logger.LogInformation("Getting balances by account");
            var result = await this.balanceService.GetAllByAccountAsync(accountId);

            if (result.HasError)
            {
                this.logger.LogWarning(
                    "Error getting balances from account {accountId} : {Message}",
                    accountId, result.Error.Message
                );
                return ErrorResponse(result.Error);
            }

            this.logger.LogInformation("Succesfully returned all balances by account");
            return ListResponse(result.Data);
        }

        /// <summary>
        /// Get all accounts of the system 
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ListResponse<AccountDto>), 200)]
        public async Task<IActionResult> GetMyAccounts()
        {
            this.logger.LogInformation("Getting all accounts");
            var result = await service.GetAllAsync();

            this.logger.LogInformation("Succesfully returned all accounts");
            return ListResponse(result.Data);
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
            this.logger.LogInformation("Starting creation of account");
            var result = await this.service.CreateAsync(account);

            if (result.HasError)
            {
                this.logger.LogWarning(
                    "Error creating account : {Message}",
                    result.Error.Message
                );
                return ErrorResponse(result.Error);
            }

            this.logger.LogInformation("Finished creation of account");
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
            this.logger.LogInformation("Starting update of account");
            var result = await service.UpdateAsync(id, account);

            if (result.HasError)
            {
                this.logger.LogWarning(
                    "Error updating account {id} : {Message}",
                    id, result.Error.Message
                );
                return ErrorResponse(result.Error);
            }

            this.logger.LogInformation("Finished update of account");
            return SaveResponse(result.Data);
        }

        /// <summary>
        /// Deletes an existing account on database
        /// </summary>
        /// <param name="id">id of the account</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteAccount([FromRoute] Guid id)
        {
            this.logger.LogInformation("Removing account");
            await service.DeleteAsync(id);
            this.logger.LogInformation("Account removal finished");

            return NoContent();
        }
    }
}
