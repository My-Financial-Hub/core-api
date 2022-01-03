using FinancialHub.Domain.Responses.Base;

namespace FinancialHub.Domain.Responses
{
    public class ValidationErrorResponse<T> : BaseResponse<T>
    {
        public ResponseError Error { get; set; }
        //TODO: maybe get a list ?
        //public List<ResponseError> Errors { get; set; }
    }
}
