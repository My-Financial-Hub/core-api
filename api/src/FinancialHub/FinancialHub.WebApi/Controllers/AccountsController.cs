using FinancialHub.Domain.Interfaces.Services;
using FinancialHub.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialHub.WebApi.Controllers
{
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class AccountsController : Controller
    {
        private readonly IAccountsService service;

        public AccountsController(IAccountsService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Obsolete("Will be changed to /me endpoint")]
        [ProducesDefaultResponseType(typeof(ICollection<AccountModel>))]
        [ProducesErrorResponseType(typeof(Exception))]
        public async Task<IActionResult> GetMyAccounts()
        {
            try
            {
                var response = await service.GetAccountsByUserAsync("mock");
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
