namespace FinancialHub.Domain.Responses.Base
{
    public abstract class BaseResponse<T>
    {
        public T Data { get; set; }
    }
}
