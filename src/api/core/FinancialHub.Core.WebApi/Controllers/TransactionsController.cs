using FinancialHub.Core.Domain.DTOS.Transactions;
using FinancialHub.Core.Domain.Filters;

namespace FinancialHub.Core.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class TransactionsController : Controller
    {
        private readonly ITransactionsService service;

        public TransactionsController(ITransactionsService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Get all transaction of the system (will be changed to only one user and added filters)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ListResponse<TransactionDto>), 200)]
        public async Task<IActionResult> GetMyTransactions([FromQuery] TransactionFilter filter)
        {
            var response = await service.GetAllByUserAsync("mock", filter);
            return Ok(new ListResponse<TransactionDto>(response.Data));
        }

        /// <summary>
        /// Creates an transaction on database (will be changed to only one user)
        /// </summary>
        /// <param name="category">Transaction to be created</param>
        [HttpPost]
        [ProducesResponseType(typeof(SaveResponse<TransactionDto>), 200)]
        [ProducesResponseType(typeof(ValidationErrorResponse), 400)]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionDto transaction)
        {
            var result = await this.service.CreateAsync(transaction);

            if (result.HasError)
            {
                return StatusCode(
                    result.Error.Code,
                    new ValidationErrorResponse(result.Error.Message)
                 );
            }

            return Ok(new SaveResponse<TransactionDto>(result.Data));
        }

        /// <summary>
        /// Updates an existing transaction on database
        /// </summary>
        /// <param name="id">id of the transaction</param>
        /// <param name="transaction">transaction changes</param>
        [NonAction]
        [Obsolete("Disabled endpoint")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SaveResponse<TransactionDto>), 200)]
        [ProducesResponseType(typeof(ValidationErrorResponse), 400)]
        public async Task<IActionResult> UpdateTransaction([FromRoute] Guid id, [FromBody] UpdateTransactionDto transaction)
        {
            var result = await this.service.UpdateAsync(id, transaction);

            if (result.HasError)
            {
                return StatusCode(
                    result.Error.Code,
                    new ValidationErrorResponse(result.Error.Message)
                 );
            }

            return Ok(new SaveResponse<TransactionDto>(result.Data));
        }

        /// <summary>
        /// Deletes an existing transaction on database
        /// </summary>
        /// <param name="id">id of the transaction</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteTransaction([FromRoute] Guid id)
        {
            await this.service.DeleteAsync(id);
            return NoContent();
        }
    }
}
