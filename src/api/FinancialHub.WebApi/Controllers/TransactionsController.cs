using FinancialHub.Core.Domain.Filters;

namespace FinancialHub.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class TransactionsController : Controller
    {
        private readonly ITransactionsService service;
        private readonly ITransactionBalanceService transactionBalanceService;

        public TransactionsController(ITransactionsService service, ITransactionBalanceService transactionBalanceService)
        {
            this.service = service;
            this.transactionBalanceService = transactionBalanceService;
        }

        /// <summary>
        /// Get all transaction of the system (will be changed to only one user and added filters)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ListResponse<TransactionModel>), 200)]
        public async Task<IActionResult> GetMyTransactions([FromQuery] TransactionFilter filter)
        {
            var response = await service.GetAllByUserAsync("mock", filter);
            return Ok(new ListResponse<TransactionModel>(response.Data));
        }

        /// <summary>
        /// Creates an transaction on database (will be changed to only one user)
        /// </summary>
        /// <param name="category">Transaction to be created</param>
        [HttpPost]
        [ProducesResponseType(typeof(SaveResponse<TransactionModel>), 200)]
        [ProducesResponseType(typeof(ValidationErrorResponse), 400)]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionModel transaction)
        {
            var result = await this.transactionBalanceService.CreateTransactionAsync(transaction);

            if (result.HasError)
            {
                return StatusCode(
                    result.Error.Code,
                    new ValidationErrorResponse(result.Error.Message)
                 );
            }

            return Ok(new SaveResponse<TransactionModel>(result.Data));
        }

        /// <summary>
        /// Updates an existing transaction on database
        /// </summary>
        /// <param name="id">id of the transaction</param>
        /// <param name="transaction">transaction changes</param>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SaveResponse<TransactionModel>), 200)]
        [ProducesResponseType(typeof(ValidationErrorResponse), 400)]
        public async Task<IActionResult> UpdateTransaction([FromRoute] Guid id, [FromBody] TransactionModel transaction)
        {
            var result = await this.service.UpdateAsync(id, transaction);

            if (result.HasError)
            {
                return StatusCode(
                    result.Error.Code,
                    new ValidationErrorResponse(result.Error.Message)
                 );
            }

            return Ok(new SaveResponse<TransactionModel>(result.Data));
        }

        /// <summary>
        /// Deletes an existing transaction on database
        /// </summary>
        /// <param name="id">id of the transaction</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteTransaction([FromRoute] Guid id)
        {
            await transactionBalanceService.DeleteTransactionAsync(id);
            return NoContent();
        }
    }
}
