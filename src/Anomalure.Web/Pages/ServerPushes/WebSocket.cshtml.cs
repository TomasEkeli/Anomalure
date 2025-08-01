using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Anomalure.Web.Pages.ServerPushes;

public partial class ServerPushesWebSocketModel(
	ILogger<ServerPushesWebSocketModel> _logger
) : PageModel
{
	public async Task OnGet()
	{
		var isHtmx = HttpContext.Request.Query["via"] == "htmx";

		var wsManager = HttpContext.WebSockets;

		if (wsManager.IsWebSocketRequest)
		{
			using WebSocket socket = await wsManager.AcceptWebSocketAsync();

			LogConnected(_logger);

			if (isHtmx)
				await StartSendingHtmlTo(
					socket,
					cancellationToken: HttpContext.RequestAborted
				);
			else
				await StartSendingJsonTo(
					socket,
					cancellationToken: HttpContext.RequestAborted
				);
		}
		else
		{
			HttpContext.Response.StatusCode = 400;
		}
	}

	async Task StartSendingHtmlTo(
		WebSocket socket,
		CancellationToken cancellationToken = default
	) => await SendData(socket, HtmlTime, cancellationToken);

	async Task StartSendingJsonTo(
		WebSocket socket,
		CancellationToken cancellationToken = default
	) => await SendData(socket, JsonTime, cancellationToken);

	public static string HtmlTime() =>
		$"""
        <div id="websocket-events">Server time is {DateTime
          .Now:yyyy-MM-dd HH:mm:ss}</div>
        """;

	public static string JsonTime() =>
		$$"""
			{
			    "serverTime": "{{DateTime.Now:yyyy-MM-ddTHH:mm:ss}}"
			}
			""";

	async Task SendData(
		WebSocket socket,
		Func<string> data,
		CancellationToken cancellationToken = default
	)
	{
		try
		{
			do
			{
				var message = data();

				await socket.SendAsync(
					buffer: new(Encoding.UTF8.GetBytes(message)),
					messageType: WebSocketMessageType.Text,
					endOfMessage: true,
					cancellationToken: cancellationToken
				);

				LogSent(_logger, message);
				await Task.Delay(Random.Shared.Next(800, 3000), cancellationToken);
			} while (
				socket.State == WebSocketState.Open
				&& !cancellationToken.IsCancellationRequested
			);
		}
		catch (Exception ex)
		{
			LogError(_logger, ex);
		}
		finally
		{
			if (socket.State == WebSocketState.Open)
			{
				try
				{
					await socket.CloseAsync(
						closeStatus: WebSocketCloseStatus.NormalClosure,
						statusDescription: "Server time sent",
						cancellationToken: CancellationToken.None
					);
				}
				catch (Exception ex)
				{
					LogCloseError(_logger, ex);
				}
			}
			LogClosed(_logger);
		}
	}
}
