using FinancialHub.Common.Results.Errors;

namespace FinancialHub.Core.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [ProducesErrorResponseType(typeof(Exception))]
    public abstract class BaseController : Controller
    {
        protected IActionResult ErrorResponse(ServiceError serviceError)
        {
            return serviceError switch
            {
                ValidationError =>
                    BadRequest(
                        error: new ValidationsErrorResponse(
                            serviceError.Message,
                            (serviceError as ValidationError).Errors
                        )
                    ),
                NotFoundError =>
                    BadRequest(
                        error: new NotFoundErrorResponse(serviceError.Message)
                    ),
                _ =>
                    StatusCode(
                        serviceError.Code,
                        new ValidationErrorResponse(serviceError.Message)
                    ),
            };
        }
    }
}
