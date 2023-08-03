namespace FinancialHub.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [ProducesErrorResponseType(typeof(Exception))]
    public class AccountsController : Controller
    {
        private readonly IAccountBalanceService accountBalanceService;
        private readonly IAccountsService service;

        public AccountsController(IAccountBalanceService accountBalanceService,IAccountsService service) 
        {
            this.accountBalanceService = accountBalanceService;
            this.service = service;
        }

        [HttpGet("{accountId}/balances")]
        [ProducesResponseType(typeof(ListResponse<BalanceModel>), 200)]
        public async Task<IActionResult> GetAccountBalances([FromRoute] Guid accountId)
        {
            var result = await this.accountBalanceService.GetBalancesByAccountAsync(accountId);

            return Ok(new ListResponse<BalanceModel>(result.Data));
        }

        /// <summary>
        /// Get all accounts of the system (will be changed to only one user)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ListResponse<AccountModel>), 200)]
        public async Task<IActionResult> GetMyAccounts()
        {
            var result = await service.GetAllByUserAsync("mock");

            return Ok(new ListResponse<AccountModel>(result.Data));
        }

        /// <summary>
        /// Creates an account on database (will be changed to only one user)
        /// </summary>
        /// <param name="account">Account to be created</param>
        [HttpPost]
        [ProducesResponseType(typeof(SaveResponse<AccountModel>), 200)]
        [ProducesResponseType(typeof(ValidationErrorResponse), 400)]
        public async Task<IActionResult> CreateAccount([FromBody] AccountModel account)
        {
            var result = await this.accountBalanceService.CreateAsync(account);

            if (result.HasError)
            {
                return StatusCode(
                    result.Error.Code, 
                    new ValidationErrorResponse(result.Error.Message)
                 );
            }

            return Ok(new SaveResponse<AccountModel>(result.Data));
        }

        /// <summary>
        /// Updates an existing account on database
        /// </summary>
        /// <param name="id">id of the account</param>
        /// <param name="account">account changes</param>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SaveResponse<AccountModel>), 200)]
        [ProducesResponseType(typeof(NotFoundErrorResponse), 404)]
        [ProducesResponseType(typeof(ValidationErrorResponse), 400)]
        public async Task<IActionResult> UpdateAccount([FromRoute] Guid id, [FromBody] AccountModel account)
        {
            var response = await service.UpdateAsync(id, account);

            if (response.HasError)
            {
                return StatusCode(
                    response.Error.Code,
                    new ValidationErrorResponse(response.Error.Message)
                 );
            }

            return Ok(new SaveResponse<AccountModel>(response.Data));
        }

        /// <summary>
        /// Deletes an existing account on database
        /// </summary>
        /// <param name="id">id of the account</param>
        [NonAction]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount([FromRoute] Guid id)
        {
            await accountBalanceService.DeleteAsync(id);
            return NoContent();
        }
    }
}
