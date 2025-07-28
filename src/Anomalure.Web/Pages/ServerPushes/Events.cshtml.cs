using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Anomalure.Web.Pages.ServerPushes;

public class ServerPushesEventsModel : PageModel
{
    public IActionResult OnGet()
    {
        Response.Headers.Append("Content-Type", "text/event-stream");
        Response.Headers.Append("Cache-Control", "no-cache");

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
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"Error in server push stream: {ex.Message}"
                    );
                }
            }
        );
    }
}
