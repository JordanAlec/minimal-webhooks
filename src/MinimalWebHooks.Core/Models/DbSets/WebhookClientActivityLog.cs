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
        TimeStamp = DateTime.Now;
        return this;
    }

    private WebhookClientActivityLog CreateWebhookLog(WebhookClient client, Action<WebhookClientActivityLog> logAction)
    {
        var currentLogs = client.ActivityLogs;
        client.ActivityLogs = null;
        logAction(this);
        TimeStamp = DateTime.Now;
        client.ActivityLogs = currentLogs;
        return this;
    }

    public WebhookClientActivityLog CreateCreateLog(WebhookClient client)
    {
        return CreateWebhookLog(client, activityLog =>
        {
            activityLog.LogType = ActivityLogType.CreatedClient;
            activityLog.Log = $"Created Client: {JsonSerializer.Serialize(client)}";
        });
    }

    public WebhookClientActivityLog CreateUpdateLog(WebhookClient client)
    {
        return CreateWebhookLog(client, activityLog =>
        {
            activityLog.LogType = ActivityLogType.UpdatedClient;
            activityLog.Log = $"Created Updated: {JsonSerializer.Serialize(client)}";
        });
    }

    public WebhookClientActivityLog CreateWebhookCallLog(WebhookClient client, WebhookActionEventResult eventResult)
    {
        var logMessage = string.IsNullOrWhiteSpace(eventResult.Message) ? "No response message" : eventResult.Message;
        return CreateWebhookLog(client, activityLog =>
        {
            activityLog.LogType = ActivityLogType.CalledWebhookUrl;
            activityLog.Log = $"Client called Url: {client.WebhookUrl}. Status code returned: {eventResult.StatusCode}. Message returned: {logMessage}. Event sent: {JsonSerializer.Serialize(eventResult.WebhookActionEvent)}";
        });
    }

}