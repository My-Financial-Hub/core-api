using Microsoft.AspNetCore.Mvc;

namespace FinancialHub.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class _CodeSmell2Controller : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var name = "aaaa";
            var res = "".Equals(name) && name.Equals("") && name.Equals(string.Empty);
            return Ok(res);
        }
    }
}
