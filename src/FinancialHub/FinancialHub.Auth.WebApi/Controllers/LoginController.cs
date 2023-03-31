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
        private readonly ITokenService tokenService;
        private readonly IUserService userService;

        public LoginController(ITokenService tokenService,IUserService userService)
        {
            this.tokenService = tokenService;
            this.userService = userService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(TokenModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Index(LoginModel login)
        {
            var userResult = await this.userService.GetAsync(login);

            if(userResult.HasError)
                return Unauthorized();
            
            var token = this.tokenService.GenerateToken(userResult.Data);

            return Ok(token);
        }
    }
}
