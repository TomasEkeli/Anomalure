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
}