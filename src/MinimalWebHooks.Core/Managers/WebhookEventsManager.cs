using MinimalWebHooks.Core.Interfaces;
using MinimalWebHooks.Core.Models;

namespace MinimalWebHooks.Core.Managers;

public class WebhookEventsManager
{
    private readonly IWebhookDataStore _dataStore;
    private readonly IWebhookActionEventProcessor _eventProcessor;
    private readonly IWebhookClientHttpClient _webhookHttpClient;

    public WebhookEventsManager(IWebhookDataStore dataStore, IWebhookActionEventProcessor eventProcessor, IWebhookClientHttpClient webhookHttpClient)
    {
        _dataStore = dataStore;
        _eventProcessor = eventProcessor;
        _webhookHttpClient = webhookHttpClient;
    }

    public async Task<bool> WriteEvent(WebhookActionEvent actionEvent) => await _eventProcessor.WriteEvent(actionEvent);

    public async Task<List<WebhookActionEventResult>> SendEvents()
    {
        var results = new List<WebhookActionEventResult>();
        if (!_eventProcessor.HasEvents()) return results;
        var events = await _eventProcessor.GetEvents();

        foreach (var actionEvent in events)
        {

            var webhookClients = await _dataStore.GetByEntity(actionEvent.Entity, actionEvent.ActionType);
            if (webhookClients == null || !webhookClients.Any()) continue;

            foreach (var webhookClient in webhookClients)
            {
                results.Add(await _webhookHttpClient.SendEventToWebhookUrl(actionEvent, webhookClient));
            }
        }

        return results;
    }
}