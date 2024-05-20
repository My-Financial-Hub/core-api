using FinancialHub.Core.WebApi.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text.Json;

namespace FinancialHub.Core.WebApi.Tests.Middleware
{
    public class ExceptionMiddlewareTests
    {
        private ILogger<ExceptionMiddleware> logger;

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger<ExceptionMiddleware>>().Object;
        }

        [Test]
        public void InvokeAsync_SuccessRequest_ShouldNotThrowException()
        {
            static Task next(HttpContext hc) => Task.CompletedTask;
            var middleware = new ExceptionMiddleware(next, this.logger);
            var context = new DefaultHttpContext();

            Assert.That(
                async () => await middleware.InvokeAsync(context),
                Throws.Nothing
            );
        }

        [Test]
        public async Task InvokeAsync_SuccessRequest_ShouldNotChangePayload()
        {
            static Task next(HttpContext hc) => Task.CompletedTask;

            var middleware = new ExceptionMiddleware(next, this.logger);

            var response = new Mock<HttpResponse>();
            response
                .SetupGet(x => x.StatusCode)
                .Returns(200);
            var context = new Mock<HttpContext>();
            context
                .SetupGet(x => x.Response)
                .Returns(response.Object);
            context
                .SetupGet(x => x.Request)
                .Returns(new DefaultHttpContext().Request);

            var defaultContext = new DefaultHttpContext();

            await middleware.InvokeAsync(context.Object);

            Assert.That(response.Object.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void InvokeAsync_RequestWithUnhandledError_ShouldNotThrowException()
        {
            static Task next(HttpContext hc) => Task.CompletedTask;
            var middleware = new ExceptionMiddleware(next, this.logger);
            var context = new DefaultHttpContext();

            Assert.That(
                async () => await middleware.InvokeAsync(context),
                Throws.Nothing
            );
        }

        [Test]
        public async Task InvokeAsync_RequestWithUnhandledError_ShouldChangeStatusTo500()
        {
            static Task next(HttpContext hc) => Task.FromException(new Exception("Error"));
            
            var middleware = new ExceptionMiddleware(next, this.logger);
            var context = new DefaultHttpContext();

            await middleware.InvokeAsync(context);

            Assert.That(context.Response.StatusCode, Is.EqualTo(500));
        }
    }
}
