using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FinancialHub.Domain.Models;
using FinancialHub.Domain.Responses;
using FinancialHub.Domain.Interfaces.Services;

namespace FinancialHub.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
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
        [ProducesResponseType(typeof(ListResponse<AccountModel>), 200)]
        public async Task<IActionResult> GetMyAccounts()
        {
            var result = await service.GetAllByUserAsync("mock");
            return Ok(new ListResponse<AccountModel>()
            {
                Data = result.Data
            });
        }

        /// <summary>
        /// Creates an account on database (will be changed to only one user)
        /// </summary>
        /// <param name="account">Account to be created</param>
        [HttpPost]
        [ProducesResponseType(typeof(SaveResponse<AccountModel>), 200)]
        [ProducesResponseType(typeof(ValidationErrorResponse<AccountModel>), 400)]
        public async Task<IActionResult> CreateAccount([FromBody] AccountModel account)
        {
            var response = await service.CreateAsync(account);

            if (response.HasError)
            {
                return StatusCode(
                    response.Error.Code, 
                    new ValidationErrorResponse<AccountModel>() { 
                        Error = new ResponseError(response.Error.Code, response.Error.Message) 
                    }
                );
            }

            return Ok(
                new SaveResponse<AccountModel>()
                {
                    Data = response.Data
                }
            );
        }

        /// <summary>
        /// Updates an existing account on database
        /// </summary>
        /// <param name="id">id of the account</param>
        /// <param name="account">account changes</param>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(AccountModel), 200)]
        public async Task<IActionResult> UpdateAccount([FromRoute] Guid id, [FromBody] AccountModel account)
        {
            var response = await service.UpdateAsync(id, account);

            if (response.HasError)
            {
                return StatusCode(response.Error.Code, new { response.Error.Message });
            }

            return Ok(response);
        }

        /// <summary>
        /// Deletes an existing account on database
        /// </summary>
        /// <param name="id">id of the account</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount([FromRoute] Guid id)
        {
            await service.DeleteAsync(id);
            return NoContent();
        }
    }
}
