using Microsoft.Extensions.Logging;
using MinimalWebHooks.Core.Enum;
using MinimalWebHooks.Core.Interfaces;
using MinimalWebHooks.Core.Models;
using MinimalWebHooks.Core.Models.DbSets;
using MinimalWebHooks.Core.Validation;
using MinimalWebHooks.Core.Validation.Rules;
using MinimalWebHooks.Core.Extensions;
using MinimalWebHooks.Core.Strategies;
using MinimalWebHooks.Core.Strategies.Updates;

namespace MinimalWebHooks.Core.Managers;

public class WebhookClientManager
{
    private readonly ILogger<WebhookClientManager> _logger;
    private readonly IWebhookDataStore _dataStore;
    private readonly IWebhookClientHttpClient _httpClient;

    public WebhookClientManager(ILogger<WebhookClientManager> logger, IWebhookDataStore dataStore, IWebhookClientHttpClient httpClient)
    {
        _logger = logger;
        _dataStore = dataStore;
        _httpClient = httpClient;
    }

    public async Task<WebhookDataResult> Get(int id, bool skipDisabledClients = true)
    {
        var client =  await _dataStore.GetById(id, skipDisabledClients);
        var found = client != null;
        _logger.LogDebug("{logger}: Searching for client Id: {id}. Found: {found}", nameof(WebhookClientManager), id, found);
        return found
            ? new WebhookDataResult().SuccessfulResult($"Client found with Id: {id}.", client)
            : new WebhookDataResult().FailedResult($"Client not found with Id: {id}.");
    }

    public async Task<WebhookDataResult> Get()
    {
        var clients = await _dataStore.Get();
        var found = clients != null && clients.Any();
        _logger.LogDebug("{logger}: Searching for all clients. Found: {found}", nameof(WebhookClientManager), found);
        return found
            ? new WebhookDataResult().SuccessfulResult($"Clients found.", clients.ToArray())
            : new WebhookDataResult().FailedResult($"No clients found.");
    }

    public async Task<WebhookDataResult> GetByEntity<T>(T data, WebhookActionType actionType)
    {
        var clients = await _dataStore.GetByEntity(data, actionType);
        var found = clients != null && clients.Any();
        _logger.LogDebug("{logger}: Searching for clients: {entityTypeName}/{actionType}. Found: {found}", data.GetEntityTypeName(), actionType, nameof(WebhookClientManager), found);
        return found
            ? new WebhookDataResult().SuccessfulResult($"Clients found.", clients.ToArray())
            : new WebhookDataResult().FailedResult($"No clients found.");
    }

    public async Task<WebhookDataResult> Create(WebhookClient client)
    {
        var validationResult = await ValidateClient(client);
        if (!validationResult.IsValid())
        {
            _logger.LogInformation("{logger}: Failed to create client ({name}): Validation. Reason: {found}", nameof(WebhookClientManager), client.Name, validationResult.GetMessage());
            return new WebhookDataResult().FailedResult(validationResult.GetMessage(), client);
        }

        var clientExists = await _dataStore.GetByName(client.Name);
        if (clientExists != null)
        {
            _logger.LogInformation("{logger}: Failed to create client ({name}): Already exists with this name. Potential duplication", nameof(WebhookClientManager), client.Name);
            return new WebhookDataResult().FailedResult($"Client already exists with this name. Potential duplication: {clientExists.Name}", clientExists);
        }
        var savedClient = await _dataStore.Create(client);
        var saved = savedClient != null;
        if (saved) await AddLogToClient(savedClient.Id, new WebhookClientActivityLog().CreateCreateLog(savedClient));
        _logger.LogInformation("{logger}: Created client ({name}): {success}", nameof(WebhookClientManager), client.Name, saved);
        return saved ? new WebhookDataResult().SuccessfulResult($"Successfully created client.", savedClient) : new WebhookDataResult().FailedResult($"Failed to created client.", client);
    }

    public async Task<WebhookDataResult> Disable(int id)
    {
        var client = await _dataStore.GetById(id);
        if (client == null)
        {
            _logger.LogInformation("{logger}: Failed to disable client. Client Id not found: {id}", nameof(WebhookClientManager), id);
            return new WebhookDataResult().FailedResult($"Client not found with Id: {id}.");
        }
        client.Disabled = true;
        var disableResult = await _dataStore.Update(client);
        if (disableResult) await AddLogToClient(client.Id, new WebhookClientActivityLog().CreateUpdateLog(client));
        _logger.LogInformation("{logger}: Disabled client ({id}): {success}", nameof(WebhookClientManager), id, disableResult);
        return disableResult
            ? new WebhookDataResult().SuccessfulResult($"Client disabled with Id: {id}.", client)
            : new WebhookDataResult().FailedResult($"Client not disabled with Id: {id}.", client);

    }

    public async Task<WebhookDataResult> Update(WebhookUpdateCommand command)
    {
        _logger.LogDebug("{logger}: Update Client ({id}): Disabling: {disabledFlag}, Replacing Headers: {replacingHeaders}", 
            nameof(WebhookClientManager), command.Id, command.SetDisabledFlag, command.HasHeaderReplacements());

        var client = await _dataStore.GetById(command.Id, skipDisabledClients: false);
        if (client == null)
        {
            _logger.LogInformation("{logger}: Failed to update client. Client Id not found: {id}", nameof(WebhookClientManager), command.Id);
            return new WebhookDataResult().FailedResult($"Client not found with Id: {command.Id}.");
        }

        var updateProcessor = new UpdateWebhookProcessor(new List<IUpdateStrategy>
        {
            new UpdateDisabledFlag(command, client),
            new ReplaceHeaders(command, client, _dataStore),
            new RemoveAllHeaders(command, client, _dataStore),
            new UpdateWebhookUrl(command, client, _httpClient)
        });

        await updateProcessor.Update();

        if (updateProcessor.HasFailures())
        {
            _logger.LogInformation("{logger}: Failed to update client ({id}): {reason}", nameof(WebhookClientManager), command.Id, updateProcessor.GetMessage());
            return new WebhookDataResult().FailedResult(updateProcessor.GetMessage());
        }


        var updateResult = await _dataStore.Update(client);
        if (updateResult) await AddLogToClient(client.Id, new WebhookClientActivityLog().CreateUpdateLog(client));

        _logger.LogInformation("{logger}: Updated client ({id}): {success}", nameof(WebhookClientManager), command.Id, updateResult);

        return updateResult
            ? new WebhookDataResult().SuccessfulResult($"Client updated with Id: {command.Id}.", client)
            : new WebhookDataResult().FailedResult($"Client not updated with Id: {command.Id}.", client);
    }

    public async Task<WebhookDataResult> AddLogToClient(int id, WebhookClientActivityLog log)
    {
        var client = await _dataStore.GetById(id, false);
        if (client == null) return new WebhookDataResult().FailedResult($"Client not found with Id: {id}.");
        client.ActivityLogs ??= new List<WebhookClientActivityLog>{ log };
        var addedLog = await _dataStore.Update(client);
        _logger.LogDebug("{logger}: Added log to client ({id}): {success}", nameof(WebhookClientManager), id, addedLog);
        return addedLog
            ? new WebhookDataResult().SuccessfulResult($"Log added to client with Id: {id}.", client)
            : new WebhookDataResult().FailedResult($"Failed to add log to client with Id: {id}.");
    }

    private async Task<WebhookValidationResult> ValidateClient(WebhookClient client)
    {
        var validator = new WebhookClientValidator(client, new List<IValidationRule>
        {
            new WebhookClientHasRequiredProps(client),
            new WebhookClientUrlCanBeReached(client, _httpClient)
        });

        return await validator.ValidateClient();
    }
}