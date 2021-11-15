using FinancialHub.Domain.Interfaces.Services;
using System.Collections.Generic;
using FinancialHub.Domain.Models;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FinancialHub.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [ProducesErrorResponseType(typeof(Exception))]
    public class AccountsController : Controller
    {
        private readonly IAccountsService service;

        public AccountsController(IAccountsService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Get all accounts of the system (will be changed to only one user)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<AccountModel>), 200)]
        public async Task<IActionResult> GetMyAccounts()
        {
            try
            {
                var response = await service.GetAllByUserAsync("mock");
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Creates an account on database (will be changed to only one user)
        /// </summary>
        /// <param name="account">Account to be created</param>
        [HttpPost]
        [ProducesResponseType(typeof(ICollection<AccountModel>),200)]
        public async Task<IActionResult> CreateAccount([FromBody] AccountModel account)
        {
            try
            {
                var response = await service.CreateAsync(account);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Updates an existing account on database
        /// </summary>
        /// <param name="id">id of the account</param>
        /// <param name="account">account changes</param>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ICollection<AccountModel>), 200)]
        public async Task<IActionResult> UpdateAccount([FromRoute]string id, [FromBody] AccountModel account)
        {
            try
            {
                var response = await service.UpdateAsync(id,account);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Deletes an existing account on database
        /// </summary>
        /// <param name="id">id of the account</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount([FromRoute] string id)
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
