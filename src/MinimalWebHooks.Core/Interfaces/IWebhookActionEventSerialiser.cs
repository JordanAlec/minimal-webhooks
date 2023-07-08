using MinimalWebHooks.Core.Models;

namespace MinimalWebHooks.Core.Interfaces;

public interface IWebhookActionEventSerialiser
{
    string GetMediaType();
    string SerialiseEvent(WebhookActionEvent actionEvent);
}