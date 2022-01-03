using FinancialHub.Domain.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FinancialHub.WebApi.Controllers.Base
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        private readonly ILogger logger;

        public BaseController(ILogger logger)
        {
            this.logger = logger;
        }

        private IActionResult CreateResponse<T>(ServiceResult<T> result)
        {
            return this.StatusCode(result.Error.Code,result.Data);
        }
    }
}
