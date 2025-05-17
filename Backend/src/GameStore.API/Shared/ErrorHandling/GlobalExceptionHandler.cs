using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;

namespace GameStore.API.Shared.ErrorHandling;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        var traceId = Activity.Current?.TraceId;

        logger.LogError(exception, "Could not process a request on the machine {MachineName}, traceId: {TraceId}",
            Environment.MachineName, traceId);

        await Results.Problem(
            title: "An error occured while processing your request.",
            statusCode: StatusCodes.Status500InternalServerError,
            extensions: new Dictionary<string, object?>
            {
                { "traceId", traceId.ToString() },
            }).ExecuteAsync(httpContext);

        return true;
    }
}