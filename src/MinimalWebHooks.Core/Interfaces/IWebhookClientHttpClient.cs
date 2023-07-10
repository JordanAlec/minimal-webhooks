using MinimalWebHooks.Core.Models;
using MinimalWebHooks.Core.Models.DbSets;

namespace MinimalWebHooks.Core.Interfaces;

public interface IWebhookClientHttpClient
{
    Task<WebhookActionEventResult> SendEventToWebhookUrl(WebhookActionEvent webhookActionEvent, WebhookClient webhookClient);
    Task<bool> VerifyWebhookUrl(WebhookClient client);
}