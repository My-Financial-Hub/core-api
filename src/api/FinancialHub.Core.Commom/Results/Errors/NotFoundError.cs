using System;

namespace FinancialHub.Common.Results.Errors
{
    public class NotFoundError : ServiceError
    {
        private const int code = 404;
        public NotFoundError(string message, Exception error = null) : base(code, message, error)
        {
        }
    }
}
