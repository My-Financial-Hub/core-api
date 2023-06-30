namespace FinancialHub.Auth.Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class SigninController : Controller
    {
        private readonly ISigninService authService;

        public SigninController(ISigninService authService)
        {
            this.authService = authService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ItemResponse<TokenModel>), 200)]
        [ProducesResponseType(typeof(ValidationErrorResponse), 400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> SigninAsync([FromBody]SigninModel login)
        {
            var tokenResult = await this.authService.AuthenticateAsync(login);

            if (tokenResult.HasError)
                return Unauthorized(
                    new ValidationErrorResponse(tokenResult.Error.Message)    
                );

            return Ok(
                new ItemResponse<TokenModel>(tokenResult.Data)
            );
        }
    }
}
