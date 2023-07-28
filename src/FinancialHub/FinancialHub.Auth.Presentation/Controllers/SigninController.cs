using Microsoft.AspNetCore.Authorization;

namespace FinancialHub.Auth.Presentation.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class SigninController : ControllerBase
    {
        private readonly ISigninService signinService;

        public SigninController(ISigninService signinService)
        {
            this.signinService = signinService;
        }

        /// <summary>
        /// Makes an Attempt to login an user
        /// </summary>
        /// <param name="login">An user credential to login</param>
        /// <response code="200">User Token after a successful login</response>
        /// <response code="400">Login fields Invalids</response>
        /// <response code="400">Email or Password Invalid</response>
        [HttpPost]
        [ProducesResponseType(typeof(ItemResponse<TokenModel>), 200)]
        [ProducesResponseType(typeof(ValidationErrorResponse), 400)]
        [ProducesResponseType(typeof(ValidationErrorResponse), 401)]
        public async Task<IActionResult> SigninAsync([FromBody]SigninModel login)
        {
            var tokenResult = await this.signinService.AuthenticateAsync(login);

            if (tokenResult.HasError)
            {
                return Unauthorized(
                    new ValidationErrorResponse(tokenResult.Error.Message)    
                );
            }

            return Ok(
                new ItemResponse<TokenModel>(tokenResult.Data)
            );
        }
    }
}
