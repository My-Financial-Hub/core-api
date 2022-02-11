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
    [ProducesErrorResponseType(typeof(Exception))]//TODO: custom exceptions
    public class TransactionsController : Controller
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
            try
            {
                var response = await service.GetAllByUserAsync("mock", filter);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ICollection<TransactionModel>), 200)]
        /// <summary>
        /// Creates an transaction on database (will be changed to only one user)
        /// </summary>
        /// <param name="category">Transaction to be created</param>
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionModel transaction)
        {
            try
            {
                var response = await service.CreateAsync(transaction);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ICollection<TransactionModel>), 200)]
        /// <summary>
        /// Updates an existing transaction on database
        /// </summary>
        /// <param name="id">id of the transaction</param>
        /// <param name="transaction">transaction changes</param>
        public async Task<IActionResult> UpdateTransaction([FromRoute] Guid id, [FromBody] TransactionModel transaction)
        {
            try
            {
                var response = await service.UpdateAsync(id, transaction);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        /// <summary>
        /// Deletes an existing transaction on database
        /// </summary>
        /// <param name="id">id of the transaction</param>
        public async Task<IActionResult> DeleteTransaction([FromRoute] Guid id)
        {
            try
            {
                await service.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
