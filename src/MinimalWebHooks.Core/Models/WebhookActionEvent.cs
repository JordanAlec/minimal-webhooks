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
    public List<WebhookActionEventUdf>? Udfs { get; private set; }

    public async Task<WebhookActionEvent> Create<T>(T data, WebhookActionType actionType, List<WebhookActionEventUdf>? udfs = null)
    {
        if (data == null) throw new ArgumentNullException(nameof(data), $"{nameof(WebhookActionEvent)} must be passed a generic type");

        ActionType = actionType;
        EventTimestamp = DateTime.Now;
        Entity = data;
        EntityTypeName = data.GetEntityTypeName();
        Source = await GetSource();
        Udfs = udfs;

        return this;
    }

    public WebhookActionEvent AddUdf(WebhookActionEventUdf eventUdf)
    {
        Udfs ??= new List<WebhookActionEventUdf>();
        Udfs.Add(eventUdf);
        return this;
    }

    private async Task<string> GetSource(string serviceUrl = "https://api.ipify.org/") => await new HttpClient().GetStringAsync(new Uri(serviceUrl));
}