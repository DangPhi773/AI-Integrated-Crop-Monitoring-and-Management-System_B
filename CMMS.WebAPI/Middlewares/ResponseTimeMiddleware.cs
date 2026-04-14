using System.Diagnostics;

namespace CMMS.WebAPI.Middlewares
{
    public class ResponseTimeMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseTimeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var watch = Stopwatch.StartNew();

            context.Response.OnStarting(() =>
            {
                watch.Stop();
                var responseTimeForMS = watch.ElapsedMilliseconds;
                context.Response.Headers["X-Response-Time-ms"] = responseTimeForMS.ToString();
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}