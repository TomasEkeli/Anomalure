using Microsoft.AspNetCore.Mvc;

namespace Anomalure.Web.Pages.ServerPushes;

public class PushStreamResult : IActionResult
{
    private readonly Func<Stream, CancellationToken, Task> _streamHandler;

    public PushStreamResult(Func<Stream, CancellationToken, Task> streamHandler)
    {
        _streamHandler = streamHandler ?? throw new ArgumentNullException(nameof(streamHandler));
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        var response = context.HttpContext.Response;
        using var stream = response.Body;
        await _streamHandler(stream, context.HttpContext.RequestAborted);
    }
}
