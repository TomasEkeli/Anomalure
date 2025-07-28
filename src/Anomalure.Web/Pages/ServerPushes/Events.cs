using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Anomalure.Web.Pages.ServerPushes;

public class ServerPushesEventsModel : PageModel
{
    public IActionResult OnGet()
    {
        Response.Headers.Append("Content-Type", "text/event-stream");
        Response.Headers.Append("Cache-Control", "no-cache");

        return new PushStreamResult(async (stream, cancellationToken) =>
        {
            var writer = new StreamWriter(stream);
            while (!cancellationToken.IsCancellationRequested)
            {
                await writer.WriteLineAsync($"data: Server time is {DateTime.Now}\n");
                await writer.FlushAsync();
                await Task.Delay(new Random().Next(1000, 3000), cancellationToken);
            }
        });
    }
}
