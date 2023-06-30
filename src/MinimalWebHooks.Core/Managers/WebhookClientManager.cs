using MinimalWebHooks.Core.Interfaces;
using MinimalWebHooks.Core.Models;

namespace MinimalWebHooks.Core.Managers;

public class WebhookClientManager
{
    private readonly IWebhookDataStore _dataStore;

    public WebhookClientManager(IWebhookDataStore dataStore) => _dataStore = dataStore;

    public async Task<WebhookDataResult> Get(int id)
    {
        var client =  await _dataStore.GetById(id);

        return client != null
            ? new WebhookDataResult().SuccessfulResult($"Client found with Id: {id}.", client)
            : new WebhookDataResult().FailedResult($"Client not found with Id: {id}.");
    }

    public async Task<WebhookDataResult> Get()
    {
        var clients = await _dataStore.Get();

        return clients != null && clients.Any()
            ? new WebhookDataResult().SuccessfulResult($"Clients found.", clients.ToArray())
            : new WebhookDataResult().FailedResult($"No clients found.");
    }

    public async Task<WebhookDataResult> Create(WebhookClient client)
    {
        var clientExists = await _dataStore.GetByName(client.Name);
        if (clientExists != null) return new WebhookDataResult().FailedResult($"Client already exists with this name. Potential duplication: {clientExists.Name}", clientExists);
        var savedClient = await _dataStore.Create(client);
        return savedClient != null ? new WebhookDataResult().SuccessfulResult($"Successfully created client.", savedClient) : new WebhookDataResult().FailedResult($"Failed to created client.", client);
    }
}