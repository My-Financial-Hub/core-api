using System;

namespace FinancialHub.Core.Common.Results.Errors
{
    public class InvalidDataError : ServiceError
    {
        private const int code = 400;
        public InvalidDataError(string message, Exception error = null) : base(code, message, error)
        {
        }
    }
}
