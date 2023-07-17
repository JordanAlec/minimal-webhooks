using MinimalWebHooks.Core.Models;

namespace MinimalWebHooks.Core.Interfaces;

public interface IWebhookActionEventProcessor
{
    Task WriteEvent(WebhookActionEvent webhookActionEvent);
    bool HasEvents();
    Task<List<WebhookActionEvent>> GetEvents();
}