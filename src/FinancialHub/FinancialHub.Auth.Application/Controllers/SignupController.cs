using Microsoft.AspNetCore.Authorization;

namespace FinancialHub.Auth.Application.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class SignupController : ControllerBase
    {
        private readonly ISignupService signupService;

        public SignupController(ISignupService signupService)
        {
            this.signupService = signupService;
        }

        /// <summary>
        /// Creates an User and a Credential
        /// </summary>
        /// <param name="signup">User and Credential informations</param>
        /// <response code="200">User and Credential Successfully created</response>
        /// <response code="400">User or Credential Validation error</response>
        [HttpPost]
        [ProducesResponseType(typeof(ItemResponse<UserModel>), 200)]
        [ProducesResponseType(typeof(ValidationErrorResponse), 400)]
        public async Task<IActionResult> SignupAsync([FromBody] SignupModel signup)
        {
            var result = await signupService.CreateAccountAsync(signup);

            if(result.HasError)
            {
                return BadRequest(
                    error: new ValidationErrorResponse(result.Error.Message)
                );
            }

            return Ok(new ItemResponse<UserModel>(result.Data));
        }
    }
}
