namespace FinancialHub.Common.Responses.Success
{
    public abstract class BaseResponse<T>
    {
        public T Data { get; }
        protected BaseResponse(T data)
        {
            this.Data = data;
        }
    }
}
