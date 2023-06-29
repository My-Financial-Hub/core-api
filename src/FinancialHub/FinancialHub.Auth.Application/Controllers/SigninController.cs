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
        [ProducesResponseType(typeof(TokenModel), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> SigninAsync([FromBody]LoginModel login)
        {
            var tokenResult = await this.authService.GenerateToken(login);

            if (tokenResult.HasError)
                return Unauthorized();

            return Ok(tokenResult.Data);
        }
    }
}
