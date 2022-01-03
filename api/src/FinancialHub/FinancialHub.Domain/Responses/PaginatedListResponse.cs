using FinancialHub.Domain.Responses.Base;
using System.Collections.Generic;

namespace FinancialHub.Domain.Responses
{
    public class PaginatedListResponse<T> : BaseResponse<IEnumerable<T>> 
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int LastPage { get; set; }
    }
}
