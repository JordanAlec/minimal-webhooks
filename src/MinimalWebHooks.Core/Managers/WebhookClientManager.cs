﻿using Microsoft.Extensions.Logging;
using MinimalWebHooks.Core.Enum;
using MinimalWebHooks.Core.Interfaces;
using MinimalWebHooks.Core.Models;
using MinimalWebHooks.Core.Models.DbSets;
using MinimalWebHooks.Core.Validation;
using MinimalWebHooks.Core.Validation.Rules;
using System;
using MinimalWebHooks.Core.Extensions;

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

    public async Task<WebhookDataResult> Get(int id)
    {
        var client =  await _dataStore.GetById(id);
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
        client.Disabled = command.SetDisabledFlag;
        if (command.HasHeaderReplacements())
        {
            if (client.ClientHeaders != null) await _dataStore.Delete(client.ClientHeaders);
            client.ClientHeaders = command.ReplaceHeaders;
        }
        var updateResult = await _dataStore.Update(client);

        _logger.LogInformation("{logger}: Updated client ({id}): {success}", nameof(WebhookClientManager), command.Id, updateResult);

        return updateResult
            ? new WebhookDataResult().SuccessfulResult($"Client updated with Id: {command.Id}.", client)
            : new WebhookDataResult().FailedResult($"Client not updated with Id: {command.Id}.", client);
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