using Microsoft.AspNetCore.Mvc;
using FinancialHub.Auth.Domain.Models;
using FinancialHub.Auth.Domain.Interfaces.Services;
using FinancialHub.Domain.Responses.Errors;
using FinancialHub.Domain.Responses.Success;

namespace FinancialHub.Auth.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService service;

        public UsersController(IUserService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Gets an user by id
        /// </summary>
        /// <param name="id">id of the User</param>
        /// <response code="200">Existing user response</response>
        /// <response code="404">Non-Existing user response</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ItemResponse<UserModel>), 200)]
        [ProducesResponseType(typeof(NotFoundErrorResponse), 404)]
        public async Task<IActionResult> GetUserAsync([FromRoute]Guid id)
        {
            var userResult = await service.GetAsync(id);

            if(userResult.HasError) 
            {
                return StatusCode(userResult.Error.Code, userResult.Error);
            }

            return Ok(userResult);
        }

        /// <summary>
        /// Creates an user 
        /// </summary>
        /// <param name="user">User data to be saved</param>
        /// <response code="200">Successful user creation</response>
        /// <response code="400">Failed user creation</response>
        [HttpPost]
        [ProducesResponseType(typeof(SaveResponse<UserModel>), 200)]
        [ProducesResponseType(typeof(ValidationErrorResponse), 400)]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserModel user)
        {
            var userResult = await service.CreateAsync(user);

            if (userResult.HasError)
            {
                return StatusCode(userResult.Error.Code, userResult.Error);
            }

            return Ok(userResult);
        }

        /// <summary>
        /// Updates an user  
        /// </summary>
        /// <param name="id">id of the User</param>
        /// <param name="user">User data to be updated (id field is ignored)</param>
        /// <response code="200">Successful update</response>
        /// <response code="400">Fail update</response>
        /// <response code="404">User not found response</response>
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(SaveResponse<UserModel>), 200)]
        [ProducesResponseType(typeof(ValidationErrorResponse), 400)]
        [ProducesResponseType(typeof(NotFoundErrorResponse), 404)]
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
