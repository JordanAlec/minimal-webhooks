using MinimalWebHooks.Core.Models;

namespace MinimalWebHooks.Core.Interfaces;

public interface IMinimalWebhookOptionsProcessor
{
    Task<bool> VerifyWebhookUrl(WebhookClient client);
}