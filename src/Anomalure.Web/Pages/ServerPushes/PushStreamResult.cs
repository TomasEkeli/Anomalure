using Microsoft.AspNetCore.Mvc;

namespace Anomalure.Web.Pages.ServerPushes;

public class PushStreamResult(
    Func<Stream, CancellationToken, Task> _streamHandler
) : IActionResult
{

    public async Task ExecuteResultAsync(
        ActionContext context
    )
    {
        ArgumentNullException.ThrowIfNull(context);

        var response = context.HttpContext.Response;
        using var stream = response.Body;
        await _streamHandler(
            stream,
            context.HttpContext.RequestAborted
        );
    }
}
