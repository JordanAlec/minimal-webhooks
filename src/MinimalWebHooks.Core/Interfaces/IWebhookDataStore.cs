using MinimalWebHooks.Core.Models;

namespace MinimalWebHooks.Core.Interfaces;

public interface IWebhookDataStore
{
    Task<List<WebhookClient>?> Get();
    Task<WebhookClient?> GetById(int id);
    Task<WebhookClient?> GetByName(string name);
    Task<WebhookClient?> Create(WebhookClient client);
}