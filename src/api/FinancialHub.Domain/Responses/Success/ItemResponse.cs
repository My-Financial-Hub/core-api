namespace FinancialHub.Domain.Responses.Success
{
    public class ItemResponse<T> : BaseResponse<T>
    {
        public ItemResponse(T data) : base(data)
        {
        }
    }
}
