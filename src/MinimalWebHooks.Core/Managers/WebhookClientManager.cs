using MinimalWebHooks.Core.Enum;
using MinimalWebHooks.Core.Interfaces;
using MinimalWebHooks.Core.Models;
using MinimalWebHooks.Core.Validation;
using MinimalWebHooks.Core.Validation.Rules;

namespace MinimalWebHooks.Core.Managers;

public class WebhookClientManager
{
    private readonly IWebhookDataStore _dataStore;
    private readonly IWebhookOptionsProcessor _optionsProcessor;

    public WebhookClientManager(IWebhookDataStore dataStore, IWebhookOptionsProcessor optionsProcessor)
    {
        _dataStore = dataStore;
        _optionsProcessor = optionsProcessor;
    }

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

    public async Task<WebhookDataResult> GetByEntity<T>(T data, WebhookActionType actionType)
    {
        var clients = await _dataStore.GetByEntity(data, actionType);

        return clients != null && clients.Any()
            ? new WebhookDataResult().SuccessfulResult($"Clients found.", clients.ToArray())
            : new WebhookDataResult().FailedResult($"No clients found.");
    }

    public async Task<WebhookDataResult> Create(WebhookClient client)
    {
        var validationResult = await ValidateClient(client);
        if (!validationResult.IsValid()) return new WebhookDataResult().FailedResult(validationResult.GetMessage(), client);

        var clientExists = await _dataStore.GetByName(client.Name);
        if (clientExists != null) return new WebhookDataResult().FailedResult($"Client already exists with this name. Potential duplication: {clientExists.Name}", clientExists);
        var savedClient = await _dataStore.Create(client);
        return savedClient != null ? new WebhookDataResult().SuccessfulResult($"Successfully created client.", savedClient) : new WebhookDataResult().FailedResult($"Failed to created client.", client);
    }

    private async Task<WebhookValidationResult> ValidateClient(WebhookClient client)
    {
        var validator = new WebhookClientValidator(client, new List<IValidationRule>
        {
            new WebhookClientHasRequiredProps(client),
            new WebhookClientUrlCanBeReached(client, _optionsProcessor)
        });

        return await validator.ValidateClient();
    }
}