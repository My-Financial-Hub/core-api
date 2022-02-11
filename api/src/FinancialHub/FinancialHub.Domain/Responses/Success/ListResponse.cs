using System.Collections.Generic;

namespace FinancialHub.Domain.Responses.Success
{
    public class ListResponse<T> : BaseResponse<ICollection<T>>
    {
        public ListResponse(ICollection<T> data) : base(data)
        {
        }
    }
}
