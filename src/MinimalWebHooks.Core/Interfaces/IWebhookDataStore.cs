using MinimalWebHooks.Core.Enum;
using MinimalWebHooks.Core.Models.DbSets;

namespace MinimalWebHooks.Core.Interfaces;

public interface IWebhookDataStore
{
    Task<List<WebhookClient>?> Get();
    Task<List<WebhookClient>?> GetByEntity<T>(T data, WebhookActionType actionType);
    Task<WebhookClient?> GetById(int id, bool skipDisabledClients = true, int includeLastNumOfLogsInMonths = 6);
    Task<WebhookClient?> GetByName(string name);
    Task<WebhookClient?> Create(WebhookClient client);
    Task<bool> Update(WebhookClient client);
    Task<bool> Delete(List<WebhookClientHeader> headers);
}