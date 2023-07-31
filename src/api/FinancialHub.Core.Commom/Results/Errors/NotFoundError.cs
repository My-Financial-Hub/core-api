using System;

namespace FinancialHub.Common.Results.Errors
{
    public class NotFoundError : ServiceError
    {
        private const int code = 404;
        public NotFoundError(string message) : base(code, message)
        {
        }
    }
}
