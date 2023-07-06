using MinimalWebHooks.Core.Models;

namespace MinimalWebHooks.Core.Interfaces;

public interface IWebhookClientHttpClient
{
    Task<WebhookActionEventResult> SendEventToWebhookUrl(WebhookActionEvent webhookActionEvent, WebhookClient webhookClient);
    Task<bool> VerifyWebhookUrl(WebhookClient client);
}