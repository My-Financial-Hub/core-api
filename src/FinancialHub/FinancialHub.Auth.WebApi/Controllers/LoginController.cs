using Microsoft.AspNetCore.Mvc;
using FinancialHub.Auth.Domain.Models;
using FinancialHub.Auth.Domain.Interfaces.Services;

namespace FinancialHub.Auth.WebApi.Controllers
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
        [ProducesResponseType(typeof(TokenModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Index(LoginModel login)
        {
            var tokenResult = await this.authService.GenerateToken(login);

            if (tokenResult.HasError)
                return Unauthorized();

            return Ok(tokenResult.Data);
        }
    }
}
