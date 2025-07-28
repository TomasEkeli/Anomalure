namespace Anomalure.Web.Pages.ServerPushes;

public partial class ServerPushesWebSocketModel
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "WebSocket connection established"
    )]
    static partial void LogConnected(
        ILogger logger
    );

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Information,
        Message = "Sent message: {Message}"
    )]
    static partial void LogSent(
        ILogger logger,
        string message
    );

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Information,
        Message = "WebSocket connection closed"
    )]
    static partial void LogClosed(
        ILogger logger
    );
}