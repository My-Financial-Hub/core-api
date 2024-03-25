using FinancialHub.Core.Domain.DTOS.Transactions;
using FinancialHub.Core.Domain.Filters;
using Microsoft.Extensions.Logging;
using System.Security.Principal;

namespace FinancialHub.Core.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class TransactionsController : BaseController
    {
        private readonly ITransactionsService service;
        private readonly ILogger<TransactionsController> logger;

        public TransactionsController(ITransactionsService service, ILogger<TransactionsController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        /// <summary>
        /// Get all transaction of the system (will be changed to only one user and added filters)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ListResponse<TransactionDto>), 200)]
        public async Task<IActionResult> GetTransactions([FromQuery] TransactionFilter filter)
        {
            this.logger.LogInformation("Getting transactions");
            var result = await service.GetAllAsync(filter);

            this.logger.LogInformation("Succesfully returned found transactions");
            return ListResponse(result.Data);
        }

        /// <summary>
        /// Creates an transaction on database (will be changed to only one user)
        /// </summary>
        /// <param name="category">Transaction to be created</param>
        [HttpPost]
        [ProducesResponseType(typeof(SaveResponse<TransactionDto>), 200)]
        [ProducesResponseType(typeof(ValidationsErrorResponse), 400)]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionDto transaction)
        {
            this.logger.LogInformation("Starting creation of transaction");
            var result = await this.service.CreateAsync(transaction);

            if (result.HasError)
            {
                this.logger.LogWarning(
                    "Error creating transaction : {Message}",
                    result.Error.Message
                );
                return ErrorResponse(result.Error);
            }

            this.logger.LogInformation("Finished creation of transaction");
            return SaveResponse(result.Data);
        }

        /// <summary>
        /// Updates an existing transaction on database
        /// </summary>
        /// <param name="id">id of the transaction</param>
        /// <param name="transaction">transaction changes</param>
        [Obsolete("Disabled endpoint")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SaveResponse<TransactionDto>), 200)]
        [ProducesResponseType(typeof(ValidationsErrorResponse), 400)]
        public async Task<IActionResult> UpdateTransaction([FromRoute] Guid id, [FromBody] UpdateTransactionDto transaction)
        {
            var result = await this.service.UpdateAsync(id, transaction);

            if (result.HasError)
            {
                return ErrorResponse(result.Error);
            }

            return SaveResponse(result.Data);
        }

        /// <summary>
        /// Deletes an existing transaction on database
        /// </summary>
        /// <param name="id">id of the transaction</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteTransaction([FromRoute] Guid id)
        {
            this.logger.LogInformation("Removing transaction");
            await service.DeleteAsync(id);
            this.logger.LogInformation("Transaction removed");

            return NoContent();
        }
    }
}
