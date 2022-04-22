using Microsoft.AspNetCore.Mvc;

namespace FinancialHub.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class CodeSmellController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var name = "aaaa";
            var res = "".Equals(name) && name.Equals("");
            return Ok(res);
        }
    }
}
