using FinancialHub.Common.Results;

namespace FinancialHub.WebApi.Controllers.Base
{
    [ApiController]
    [System.Obsolete("Maybe use a middleware")]
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult CreateResponse<T>(ServiceResult<T> result)
        {
            return this.StatusCode(result.Error.Code,result.Data);
        }

        protected IActionResult SuccessListResponse<T>(ServiceResult<ICollection<T>> result)
        {
            return this.Ok(new ListResponse<T>(result.Data)); 
        }

        protected IActionResult SuccessSaveResponse<T>(ServiceResult<T> result)
        {
            return this.Ok(new SaveResponse<T>(result.Data)); 
        }
    }
}
