using Microsoft.Extensions.Logging;
using MinimalWebHooks.Core.Interfaces;
using MinimalWebHooks.Core.Models;

namespace MinimalWebHooks.Core.Managers;

public class WebhookEventsManager
{
    private readonly ILogger<WebhookEventsManager> _logger;
    private readonly IWebhookDataStore _dataStore;
    private readonly IWebhookActionEventProcessor _eventProcessor;
    private readonly IWebhookClientHttpClient _webhookHttpClient;

    public WebhookEventsManager(ILogger<WebhookEventsManager> logger, IWebhookDataStore dataStore, IWebhookActionEventProcessor eventProcessor, IWebhookClientHttpClient webhookHttpClient)
    {
        _logger = logger;
        _dataStore = dataStore;
        _eventProcessor = eventProcessor;
        _webhookHttpClient = webhookHttpClient;
    }

    public async Task WriteEvent(WebhookActionEvent actionEvent)
    {
        _logger.LogInformation("{logger}: Writing event for {entityTypeName}/{actionType}", nameof(WebhookEventsManager), actionEvent.EntityTypeName, actionEvent.ActionType);
        await _eventProcessor.WriteEvent(actionEvent);
    }

    public async Task<List<WebhookActionEventResult>> SendEvents()
    {
        var results = new List<WebhookActionEventResult>();
        if (!_eventProcessor.HasEvents()) return results;
        var events = await _eventProcessor.GetEvents();

        _logger.LogInformation("{logger}: Prepping to send events: {eventCount}", nameof(WebhookEventsManager), events.Count);

        foreach (var actionEvent in events)
        {
            _logger.LogInformation("{logger}: Event: {entityTypeName}/{actionType}", nameof(WebhookEventsManager), actionEvent.EntityTypeName, actionEvent.ActionType);
            var webhookClients = await _dataStore.GetByEntity(actionEvent.Entity, actionEvent.ActionType);
            _logger.LogInformation("{logger}: Clients found for event: {clientCount}", nameof(WebhookEventsManager), webhookClients?.Count);
            if (webhookClients == null || !webhookClients.Any()) continue;

            foreach (var webhookClient in webhookClients)
            {
                var result = await _webhookHttpClient.SendEventToWebhookUrl(actionEvent, webhookClient);
                _logger.LogInformation("{logger}: Sent event: {clientSuccess}. Client name: {clientName}", nameof(WebhookEventsManager), result.Success, result.WebhookClient.Name);
                results.Add(result);
            }
        }

        return results;
    }
}