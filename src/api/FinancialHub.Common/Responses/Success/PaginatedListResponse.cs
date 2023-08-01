using System.Collections.Generic;

namespace FinancialHub.Common.Responses.Success
{
    public class PaginatedListResponse<T> : BaseResponse<IEnumerable<T>> 
    {
        public int Page { get; }
        public int PageSize { get; }
        public int LastPage { get; }

        public PaginatedListResponse(IEnumerable<T> data, int page, int pageSize, int lastPage) : base(data)
        {
            Page = page;
            PageSize = pageSize;
            LastPage = lastPage;
        }
    }
}
