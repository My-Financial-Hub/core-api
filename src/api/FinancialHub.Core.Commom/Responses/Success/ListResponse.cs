using System.Collections.Generic;

namespace FinancialHub.Core.Common.Responses.Success
{
    public class ListResponse<T> : BaseResponse<ICollection<T>>
    {
        public ListResponse(ICollection<T> data) : base(data)
        {
        }
    }
}
