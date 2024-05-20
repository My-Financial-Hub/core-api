using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FinancialHub.Core.WebApi.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var method = context.Request.Method.ToUpper();
            var path = context.Request.Path.Value;
            try
            {
                this.logger.LogInformation("[{request}] - {path} Started", method, path);
                await next(context);
            }
            catch (Exception exception)
            {
                this.logger.LogError(
                    exception,
                    "[{request}] - {path} error with message {message}",
                    method, path, exception.Message
                );
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(
                    new
                    {
                        HasError = true,
                        Error = new
                        {
                            Code = 500,
                            exception.Message,
                        }
                    }
                );
            }
            finally
            {
                var status = context.Response.StatusCode;
                this.logger.LogInformation("[{request}] - {path} Finished with status {status}", method, path, status);
            }
        }
    }
}
