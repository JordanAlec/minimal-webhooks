using MinimalWebHooks.Core.Models.DbSets;

namespace MinimalWebHooks.Core.Interfaces;

public interface IWebhookOptionsProcessor
{
    Task<bool> VerifyWebhookUrl(WebhookClient client);
}