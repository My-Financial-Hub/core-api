namespace FinancialHub.Auth.Application.Controllers
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
                return StatusCode(
                    userResult.Error.Code, 
                    new NotFoundErrorResponse(userResult.Error.Message)                    
                );
            }

            return Ok(
                new ItemResponse<UserModel>(userResult.Data)
            );
        }

        /// <summary>
        /// Creates an user 
        /// </summary>
        /// <param name="user">User data to be saved</param>
        /// <response code="200">Successful user creation</response>
        /// <response code="400">Failed user creation</response>
        [Obsolete("removed : use /sign-up")]
        [HttpPost]
        [ProducesResponseType(typeof(SaveResponse<UserModel>), 200)]
        [ProducesResponseType(typeof(ValidationErrorResponse), 400)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Info Code Smell", "S1133:Deprecated code should be removed", Justification = "In Progress")]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserModel user)
        {
            var userResult = await service.CreateAsync(user);

            if (userResult.HasError)
            {
                return StatusCode(
                    userResult.Error.Code,
                    new ValidationErrorResponse(userResult.Error.Message)
                );
            }

            return Ok(new SaveResponse<UserModel>(userResult.Data));
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
                if(userResult.Error.Code == 404)
                {
                    return NotFound(
                        new NotFoundErrorResponse(userResult.Error.Message)
                    );
                }

                return BadRequest(
                    new ValidationErrorResponse(userResult.Error.Message)
                );
            }

            return Ok(new SaveResponse<UserModel>(userResult.Data));
        }
    }
}
