namespace FinancialHub.Domain.Responses.Success
{
    public abstract class BaseResponse<T>
    {
        public T Data { get; set; }
    }
}
