using MinimalWebHooks.Core.Interfaces;
using MinimalWebHooks.Core.Models;
using System.Text.Json;

namespace MinimalWebHooks.Core.Serialisation;

public class DefaultWebhookActionEventSerialiser : IWebhookActionEventSerialiser
{
    public string GetMediaType() => "application/json";

    public string SerialiseEvent(WebhookActionEvent actionEvent) => JsonSerializer.Serialize(actionEvent);
}