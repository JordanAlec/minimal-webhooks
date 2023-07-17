using System.Reflection.PortableExecutable;
using System.Threading;
using System.Threading.Channels;
using MinimalWebHooks.Core.Extensions;
using MinimalWebHooks.Core.Interfaces;
using MinimalWebHooks.Core.Models;

namespace MinimalWebHooks.Core.Processors;

public class WebhookActionEventProcessor : IWebhookActionEventProcessor
{
    private readonly int _maxCapacity;
    private readonly Channel<WebhookActionEvent> _webhookActionEventChannel;

    public WebhookActionEventProcessor(WebhookOptions options)
    {
        _maxCapacity = options.EventOptions!.QueueCapacity;
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
        await _webhookActionEventChannel.Reader.WaitToReadAsync();
        var batch = new List<WebhookActionEvent>();
        while (batch.Count < _maxCapacity && _webhookActionEventChannel.Reader.TryRead(out var actionEvent)) batch.Add(actionEvent);
        return batch;
    }
}