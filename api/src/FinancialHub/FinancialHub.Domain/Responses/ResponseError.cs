namespace FinancialHub.Domain.Responses
{
    public class ResponseError
    {
        public int Code { get; protected set; }
        public string Message { get; protected set; }

        public ResponseError(int code, string message)
        {
            this.Code = code;
            this.Message = message;
        }
    }
}
