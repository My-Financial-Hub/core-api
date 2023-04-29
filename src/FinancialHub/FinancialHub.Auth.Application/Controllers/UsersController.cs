using FinancialHub.Auth.Domain.Interfaces.Services;
using FinancialHub.Auth.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinancialHub.Auth.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService service;

        public UsersController(IUserService service)
        {
            this.service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserAsync([FromRoute]Guid id)
        {
            var userResult = await service.GetAsync(id);

            if(userResult.HasError) 
            {
                return StatusCode(userResult.Error.Code, userResult.Error);
            }

            return Ok(userResult);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UserModel user)
        {
            var userResult = await service.CreateAsync(user);

            if (userResult.HasError)
            {
                return StatusCode(userResult.Error.Code, userResult.Error);
            }

            return Ok(userResult);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUserAsync([FromRoute] Guid id, [FromBody]UserModel user)
        {
            var userResult = await service.UpdateAsync(id, user);

            if (userResult.HasError)
            {
                return StatusCode(userResult.Error.Code, userResult.Error);
            }

            return Ok(userResult);
        }
    }
}
