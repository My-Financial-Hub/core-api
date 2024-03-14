namespace FinancialHub.Common.Responses.Errors
{
    public class ValidationsErrorResponse : BaseErrorResponse
    {
        public FieldValidationErrorResponse[] Errors { get; protected set; }

        public ValidationsErrorResponse(string message, FieldValidationErrorResponse[] errors) : base(400, message)
        {
            this.Errors = errors;
        }

        public class FieldValidationErrorResponse
        {
            public string Field { get; protected set; }
            public string[] Messages { get; protected set; }

            public FieldValidationErrorResponse(string field, string[] messages)
            {
                this.Field = field;
                this.Messages = messages;
            }
        }
    }
}
