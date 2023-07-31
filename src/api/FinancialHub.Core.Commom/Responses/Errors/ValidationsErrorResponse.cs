using System.Collections.Generic;

namespace FinancialHub.Common.Responses.Errors
{
    public class ValidationsErrorResponse : BaseErrorResponse
    {
        public Dictionary<string, string[]> Errors { get; set; }

        public ValidationsErrorResponse(string message, Dictionary<string, string[]> errors = null) 
            : base(400, message)
        { 
            this.Errors = errors;
        }
    }
}
