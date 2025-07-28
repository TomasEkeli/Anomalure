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
                cancellationToken: HttpContext.RequestAborted
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
        try
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
                        cancellationToken: cancellationToken
                    );

                LogSent(_logger, message);
                await Task.Delay(
                    Random.Shared.Next(800, 3000),
                    cancellationToken
                );

            } while (
                webSocket.State == WebSocketState.Open &&
                !cancellationToken.IsCancellationRequested
            );
        }
        catch (Exception ex)
        {
            LogError(
                _logger,
                ex
            );
        }
        finally
        {
            if (webSocket.State == WebSocketState.Open)
            {
                try
                {
                    await webSocket.CloseAsync(
                        closeStatus: WebSocketCloseStatus.NormalClosure,
                        statusDescription: "Server time sent",
                        cancellationToken: CancellationToken.None
                    );
                }
                catch (Exception ex)
                {
                    LogCloseError(
                        _logger,
                        ex
                    );
                }
            }
            LogClosed(_logger);
        }
    }
}
