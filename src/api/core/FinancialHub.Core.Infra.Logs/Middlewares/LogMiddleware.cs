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
                this.logger.LogError(
                    exception,
                    "[{request}] - {path} error with message {message}",
                    method, path, exception.Message
                );
                throw;
            }
            finally
            {
                this.logger.LogInformation("[{request}] - {path} Finished", method, path);
            }
        }
    }
}
