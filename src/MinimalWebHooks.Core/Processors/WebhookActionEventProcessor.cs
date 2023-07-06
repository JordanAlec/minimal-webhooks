using System.Threading.Channels;
using MinimalWebHooks.Core.Interfaces;
using MinimalWebHooks.Core.Models;

namespace MinimalWebHooks.Core.Processors;

public class WebhookActionEventProcessor : IWebhookActionEventProcessor
{
    private readonly Channel<WebhookActionEvent> _webhookActionEventChannel;

    public WebhookActionEventProcessor() => _webhookActionEventChannel = Channel.CreateBounded<WebhookActionEvent>(5);

    public async Task<bool> WriteEvent(WebhookActionEvent webhookActionEvent)
    {
        while (await _webhookActionEventChannel.Writer.WaitToWriteAsync())
            _webhookActionEventChannel.Writer.TryWrite(webhookActionEvent);

        return _webhookActionEventChannel.Writer.TryComplete();
    }

    public bool HasEvents() => 
        _webhookActionEventChannel.Reader.CanCount && _webhookActionEventChannel.Reader.Count > 0;

    public async Task<List<WebhookActionEvent>> GetEvents()
    {
        var events = new List<WebhookActionEvent>();
        var webhookActionEvents = _webhookActionEventChannel.Reader.ReadAllAsync();
        await foreach (var webhookActionEvent in webhookActionEvents) events.Add(webhookActionEvent);
        return events;
    }
}