using System.Diagnostics;

namespace GameStore.API.Shared.Timing;

public class RequestTimingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ILogger<RequestTimingMiddleware> logger)
    {
        var stopWatch = new Stopwatch();
        try
        {
            stopWatch.Start();
            await next(context);
        }
        finally
        {
            stopWatch.Stop();
            logger.LogInformation(
                "{RequestMethod} {RequestPath} completed with status {ResponseStatus} and took {milliseconds} milliseconds.",
                context.Request.Method, context.Request.Path, context.Response.StatusCode, stopWatch.ElapsedMilliseconds);
        }
    }
}