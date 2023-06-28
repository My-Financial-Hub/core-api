namespace FinancialHub.Auth.Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class LoginController : Controller
    {
        private readonly IAuthService authService;

        public LoginController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(TokenModel), 200)]
        public async Task<IActionResult> Index(LoginModel login)
        {
            var tokenResult = await this.authService.GenerateToken(login);

            if (tokenResult.HasError)
                return Unauthorized();

            return Ok(tokenResult.Data);
        }
    }
}
