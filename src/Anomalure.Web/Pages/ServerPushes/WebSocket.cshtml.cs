using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.WebSockets;
using System.Text;

namespace Anomalure.Web.Pages.ServerPushes;

public partial class ServerPushesWebSocketModel(
    ILogger<ServerPushesWebSocketModel> _logger
) : PageModel
{
    public async Task OnGet()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext
                .WebSockets
                .AcceptWebSocketAsync();

            LogConnected(_logger);

            await EndlesslySendTime(
                webSocket,
                CancellationToken.None
            );
        }
        else
        {
            HttpContext.Response.StatusCode = 400;
        }
    }

    async Task EndlesslySendTime(
        WebSocket webSocket,
        CancellationToken cancellationToken = default
    )
    {
        do
        {
            var message =
            $"""
            <div id="websocket-events">Server time is {DateTime.Now:yyyy-MM-dd HH:mm:ss}</div>
            """;
            var echoMessage = Encoding.UTF8.GetBytes(message);

            await webSocket
                .SendAsync(
                    buffer: new(echoMessage),
                    messageType: WebSocketMessageType.Text,
                    endOfMessage: true,
                    cancellationToken: CancellationToken.None
                );

            LogSent(_logger, message);
            await Task.Delay(
                Random.Shared.Next(800, 3000)
            );

        } while (
            webSocket.State == WebSocketState.Open &&
            !cancellationToken.IsCancellationRequested
        );
        await webSocket
            .CloseAsync(
                closeStatus: WebSocketCloseStatus.NormalClosure,
                statusDescription: "Server time sent",
                cancellationToken: CancellationToken.None
            );
        LogClosed(_logger);
    }
}
