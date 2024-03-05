using static FinancialHub.Common.Results.Errors.ValidationError;

namespace FinancialHub.Common.Responses.Errors
{
    public class ValidationsErrorResponse : BaseErrorResponse
    {
        public FieldValidationErrorResponse[] Errors { get; protected set; }

        public ValidationsErrorResponse(string message, FieldValidationError[] errors) : base(400, message)
        {
            this.Errors = errors.Select(
                e => new FieldValidationErrorResponse(
                    e.Field, 
                    e.Messages
                )
            ).ToArray();
        }

        public class FieldValidationErrorResponse
        {
            public string Field { get; }
            public string[] Messages { get; }

            public FieldValidationErrorResponse(string field, string[] messages)
            {
                this.Field = field;
                this.Messages = messages;
            }
        }
    }
}
