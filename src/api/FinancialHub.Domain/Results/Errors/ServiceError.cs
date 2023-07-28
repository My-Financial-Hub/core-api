using System;

namespace FinancialHub.Domain.Results.Errors
{
    public class ServiceError
    {
        #warning it can be changed to an enum or use a different value from https codes
        public int Code { get; protected set; }
        public string Message { get; protected set; }
        public Exception Error { get; protected set; }

        public ServiceError(int code,string message, Exception error = null)
        {
            this.Code = code;
            this.Message = message;
            this.Error = error;
        }
    }
}
