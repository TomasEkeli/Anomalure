using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Anomalure.Web.Pages.ServerPushes;

public partial class ServerPushesEventsModel(
    ILogger<ServerPushesEventsModel> _logger
) : PageModel
{
    public IActionResult OnGet()
    {
        SetServerPushHeaders(Response.Headers);

        return new PushStreamResult(
            async (stream, cancellationToken) =>
            {
                try
                {
                    using StreamWriter writer = new(
                        stream: stream,
                        leaveOpen: true,
                        encoding: System.Text.Encoding.UTF8
                    );

                    while (!cancellationToken.IsCancellationRequested)
                    {
                        await writer
                            .WriteLineAsync(
                                $"data: Server time is {DateTime.Now}\n"
                            );
                        await writer.FlushAsync(
                            cancellationToken
                        );
                        await Task.Delay(
                            Random.Shared.Next(800, 3000),
                            cancellationToken
                        );
                    }
                }
                catch (TaskCanceledException) { }
                catch (InvalidOperationException) { }
                catch (Exception ex)
                {
                    LogError(_logger, ex);
                }
            }
        );
    }

    static void SetServerPushHeaders(IHeaderDictionary headers)
    {
        headers.Append("Content-Type", "text/event-stream");
        headers.Append("Cache-Control", "no-cache");
    }
}
