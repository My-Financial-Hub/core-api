using FinancialHub.Domain.Responses.Base;
using System.Collections.Generic;

namespace FinancialHub.Domain.Responses
{
    public class ListResponse<T> : BaseResponse<IEnumerable<T>>
    {

    }
}
