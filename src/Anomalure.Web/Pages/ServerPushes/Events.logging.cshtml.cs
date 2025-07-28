namespace Anomalure.Web.Pages.ServerPushes;

public partial class ServerPushesEventsModel
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "Error in server push stream"
    )]
    static partial void LogError(
        ILogger logger,
        Exception ex
    );

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Information,
        Message = "Server push stream cancelled"
    )]
    static partial void LogCancelled(
        ILogger logger,
        TaskCanceledException ex
    );

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Information,
        Message = "Invalid operation in server push stream"
    )]
    static partial void LogInvalidOperation(
        ILogger logger,
        InvalidOperationException ex
    );

}