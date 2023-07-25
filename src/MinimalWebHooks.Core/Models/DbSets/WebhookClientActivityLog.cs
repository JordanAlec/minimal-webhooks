using System.Net;
using MinimalWebHooks.Core.Enum;
using System.Text.Json;

namespace MinimalWebHooks.Core.Models.DbSets;

public class WebhookClientActivityLog
{
    public int Id { get; set; }
    public ActivityLogType LogType { get; private set; }
    public string Log { get; private set; }
    public DateTime TimeStamp { get; private set; }

    public WebhookClientActivityLog CreateLog(ActivityLogType type, string log)
    {
        LogType = type;
        Log = log;
        TimeStamp = DateTime.Now.ToUniversalTime();
        return this;
    }

    private WebhookClientActivityLog CreateWebhookLog(WebhookClient client, Action<WebhookClientActivityLog> logAction)
    {
        var currentLogs = client.ActivityLogs;
        client.ActivityLogs = null;
        logAction(this);
        client.ActivityLogs = currentLogs;
        return this;
    }

    public WebhookClientActivityLog CreateCreateLog(WebhookClient client)
    {
        return CreateWebhookLog(client, activityLog =>
        {
            activityLog.LogType = ActivityLogType.CreatedClient;
            activityLog.Log = "Created Client";
            activityLog.TimeStamp = DateTime.Now.ToUniversalTime();
        });
    }

    public WebhookClientActivityLog CreateUpdateLog(WebhookClient client)
    {
        return CreateWebhookLog(client, activityLog =>
        {
            activityLog.LogType = ActivityLogType.UpdatedClient;
            activityLog.Log = $"Created Updated: {JsonSerializer.Serialize(client)}";
            activityLog.TimeStamp = DateTime.Now.ToUniversalTime();
        });
    }

    public WebhookClientActivityLog CreateWebhookCallLog(WebhookClient client, int statusCode, string? message)
    {
        return CreateWebhookLog(client, activityLog =>
        {
            activityLog.LogType = ActivityLogType.CalledWebhookUrl;
            activityLog.Log = $"Client called Url: {JsonSerializer.Serialize(client)}. Status code returned: {statusCode}. Message: {message}";
            activityLog.TimeStamp = DateTime.Now.ToUniversalTime();
        });
    }

}