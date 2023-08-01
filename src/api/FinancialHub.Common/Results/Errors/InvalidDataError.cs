using System;

namespace FinancialHub.Common.Results.Errors
{
    public class InvalidDataError : ServiceError
    {
        private const int code = 400;
        public InvalidDataError(string message) : base(code, message)
        {
        }
    }
}
