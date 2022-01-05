namespace FinancialHub.Domain.Responses.Errors
{
    public class ValidationErrorResponse : BaseErrorResponse
    {
        public ValidationErrorResponse(string message) : base(400, message)
        {
        }
    }
}
