using System;

namespace FinancialHub.Domain.Results.Errors
{
    public class InvalidDataError : ServiceError
    {
        private const int code = 400;
        public InvalidDataError(string message, Exception error = null) : base(code, message, error)
        {
        }
    }
}
