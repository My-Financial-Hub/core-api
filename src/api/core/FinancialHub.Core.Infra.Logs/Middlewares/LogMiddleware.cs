using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FinancialHub.Core.Infra.Logs.Middlewares
{
    public class LogMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<LogMiddleware> logger;

        public LogMiddleware(RequestDelegate next, ILogger<LogMiddleware> logger)
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
                var status = context.Response.StatusCode;
                this.logger.LogError(
                    exception,
                    "[{request}] - {path} error with message {message} and status {status}",
                    method, path, exception.Message, status
                );
                throw;
            }
            finally
            {
                var status = context.Response.StatusCode;
                this.logger.LogInformation("[{request}] - {path} Finished with status {status}", method, path, status);
            }
        }
    }
}
