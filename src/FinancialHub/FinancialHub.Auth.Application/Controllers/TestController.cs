using Microsoft.AspNetCore.Authorization;

namespace FinancialHub.Auth.Application.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult TestOk()
        {
            return Ok();
        }
    }
}
