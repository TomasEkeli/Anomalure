namespace Anomalure.Web.Pages.ServerPushes;

public partial class ServerPushesWebSocketModel
{
	[LoggerMessage(
		EventId = 1,
		Level = LogLevel.Information,
		Message = "WebSocket connection established"
	)]
	static partial void LogConnected(ILogger logger);

	[LoggerMessage(
		EventId = 2,
		Level = LogLevel.Information,
		Message = "Sent message: {Message}"
	)]
	static partial void LogSent(ILogger logger, string message);

	[LoggerMessage(
		EventId = 3,
		Level = LogLevel.Information,
		Message = "WebSocket connection closed"
	)]
	static partial void LogClosed(ILogger logger);

	[LoggerMessage(
		EventId = 4,
		Level = LogLevel.Error,
		Message = "WebSocket error"
	)]
	static partial void LogError(ILogger logger, Exception exception);

	[LoggerMessage(
		EventId = 5,
		Level = LogLevel.Debug,
		Message = "Closing websocket errored, probably already closed"
	)]
	static partial void LogCloseError(ILogger logger, Exception exception);
}
