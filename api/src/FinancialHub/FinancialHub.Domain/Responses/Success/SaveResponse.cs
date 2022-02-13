namespace FinancialHub.Domain.Responses.Success
{
    public class SaveResponse<T> : BaseResponse<T>
    {
        public SaveResponse(T data) : base(data)
        {
        }
    }
}
