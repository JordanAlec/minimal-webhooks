using System.Threading.Channels;

namespace MinimalWebHooks.Core.Models;

public class WebhookEventOptions
{
    public int QueueCapacity;
    public BoundedChannelFullMode FullMode;
}