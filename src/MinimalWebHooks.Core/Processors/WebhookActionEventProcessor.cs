using System.Threading.Channels;
using Microsoft.Extensions.Options;
using MinimalWebHooks.Core.Interfaces;
using MinimalWebHooks.Core.Models;

namespace MinimalWebHooks.Core.Processors;

public class WebhookActionEventProcessor : IWebhookActionEventProcessor
{
    private readonly Channel<WebhookActionEvent> _webhookActionEventChannel;

    public WebhookActionEventProcessor(WebhookOptions options)
    {
        _webhookActionEventChannel = Channel.CreateBounded<WebhookActionEvent>(new BoundedChannelOptions(options.EventOptions!.QueueCapacity)
        {
            SingleWriter = true,
            SingleReader = true,
            FullMode = options.EventOptions!.FullMode
        });
    }

    public async Task WriteEvent(WebhookActionEvent webhookActionEvent) => 
        await _webhookActionEventChannel.Writer.WriteAsync(webhookActionEvent);

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