using MinimalWebHooks.Core.Enum;
using MinimalWebHooks.Core.Extensions;

namespace MinimalWebHooks.Core.Models;

public class WebhookActionEvent
{
    public WebhookActionType ActionType { get; private set; }
    public DateTime EventTimestamp { get; private set; }
    public object? Entity { get; private set; }
    public string? EntityTypeName { get; private set; }
    public string? Source { get; private set; }

    public async Task<WebhookActionEvent> CreateEvent<T>(T data, WebhookActionType actionType)
    {
        if (data == null) throw new ArgumentNullException(nameof(data), $"{nameof(WebhookActionEvent)} must be passed a generic type");

        ActionType = actionType;
        EventTimestamp = DateTime.Now;
        Entity = data;
        EntityTypeName = data.GetEntityTypeName();
        Source = await GetSource();

        return this;
    }

    private async Task<string> GetSource(string serviceUrl = "https://api.ipify.org/") => await new HttpClient().GetStringAsync(new Uri(serviceUrl));
}