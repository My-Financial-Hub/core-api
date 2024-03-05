namespace FinancialHub.Common.Results.Errors
{
    public class ValidationError : ServiceError
    {
        public FieldValidationError[] Errors { get; protected set; }

        public ValidationError(string message, FieldValidationError[] errors) : base(400, message)
        {
            this.Errors = errors;
        }

        public class FieldValidationError
        {
            public string Field { get; }
            public string[] Messages { get; }

            public FieldValidationError(string field, string[] messages)
            {
                this.Field = field;
                this.Messages = messages;
            }
        }
    }
}
