using System;

namespace FinancialHub.Domain.Results.Errors
{
    public class NotFoundServiceError : ServiceError
    {
        private const int code = 404;
        public NotFoundServiceError(string message, Exception error = null) : base(code, message, error)
        {
        }
    }
}
