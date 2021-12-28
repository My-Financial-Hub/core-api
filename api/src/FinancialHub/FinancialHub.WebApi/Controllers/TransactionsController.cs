using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FinancialHub.Domain.Models;
using System.Collections.Generic;
using FinancialHub.Domain.Interfaces.Services;
using FinancialHub.Domain.Filters;

namespace FinancialHub.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class TransactionsController : Controller//TODO: base controller & base responses
    {
        private readonly ITransactionsService service;

        public TransactionsController(ITransactionsService service)
        {
            this.service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ICollection<TransactionModel>), 200)]
        /// <summary>
        /// Get all transaction of the system (will be changed to only one user and added filters)
        /// </summary>
        public async Task<IActionResult> GetMyTransactions(
            [FromQuery] TransactionFilter filter
        )
        {
            var response = await service.GetAllByUserAsync("mock", filter);
            return Ok(response.Data);
        }

        [HttpPost]
        [ProducesResponseType(typeof(TransactionModel), 200)]
        /// <summary>
        /// Creates an transaction on database (will be changed to only one user)
        /// </summary>
        /// <param name="category">Transaction to be created</param>
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionModel transaction)
        {
            var response = await service.CreateAsync(transaction);
            return Ok(response.Data);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TransactionModel), 200)]
        /// <summary>
        /// Updates an existing transaction on database
        /// </summary>
        /// <param name="id">id of the transaction</param>
        /// <param name="transaction">transaction changes</param>
        public async Task<IActionResult> UpdateTransaction([FromRoute] Guid id, [FromBody] TransactionModel transaction)
        {
            var response = await service.UpdateAsync(id, transaction);

            if (response.HasError)
            {
                return StatusCode(response.Error.Code, new { response.Error.Message });
            }

            return Ok(response.Data);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        /// <summary>
        /// Deletes an existing transaction on database
        /// </summary>
        /// <param name="id">id of the transaction</param>
        public async Task<IActionResult> DeleteTransaction([FromRoute] Guid id)
        {
            await service.DeleteAsync(id);
            return NoContent();
        }
    }
}
