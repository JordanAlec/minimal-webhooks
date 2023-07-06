using MinimalWebHooks.Core.Models;

namespace MinimalWebHooks.Core.Interfaces;

public interface IWebhookOptionsProcessor
{
    Task<bool> VerifyWebhookUrl(WebhookClient client);
}