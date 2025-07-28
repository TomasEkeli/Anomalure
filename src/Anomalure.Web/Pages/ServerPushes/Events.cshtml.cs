using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Anomalure.Web.Pages.ServerPushes;

public partial class ServerPushesEventsModel(
    ILogger<ServerPushesEventsModel> _logger
) : PageModel
{
    public IActionResult OnGet()
    {
        var isHtmx = Request.Query["via"] == "htmx";
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
                        var data = $"Server time is {DateTime
                            .Now:yyyy-MM-dd HH:mm:ss}";
                        if (isHtmx)
                        {
                            await WriteHtmlEvent(
                                writer,
                                data: data
                            );
                        }
                        else
                        {
                            await WriteTextEvent(
                                writer,
                                data: data
                            );
                        }
                        await Task.Delay(
                            Random.Shared.Next(800, 3000),
                            cancellationToken
                        );
                    }
                }
                catch (TaskCanceledException ex)
                {
                    LogCancelled(_logger, ex);
                }
                catch (InvalidOperationException ex)
                {
                    LogInvalidOperation(_logger, ex);
                }
                catch (Exception ex)
                {
                    LogError(_logger, ex);
                    throw;
                }
            }
        );
    }

    static void SetServerPushHeaders(
        IHeaderDictionary headers
    )
    {
        headers.Append(
            "Content-Type", "text/event-stream"
        );
        headers.Append(
            "Cache-Control", "no-cache"
        );
    }

    static async Task WriteHtmlEvent(
        StreamWriter writer,
        string data
    ) =>
        await WriteEvent(
            writer,
            data: "<div>" + data + "</div>"
        );

    static async Task WriteTextEvent(
        StreamWriter writer,
        string data
    ) =>
        await WriteEvent(
            writer,
            data: data
        );

    static async Task WriteEvent(
        StreamWriter writer,
        string data,
        string eventType = "message"
    )
    {
        await writer.WriteLineAsync(
            "event: " + eventType
        );
        await writer.WriteLineAsync(
            "data: " + data
        );
        await writer.WriteLineAsync();
        await writer.FlushAsync();
    }
}
