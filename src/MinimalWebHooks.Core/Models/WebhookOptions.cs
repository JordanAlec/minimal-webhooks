using MinimalWebHooks.Core.Interfaces;

namespace MinimalWebHooks.Core.Models;

public class WebhookOptions
{
    public bool VerifyWebhookUrl;
    public IWebhookActionEventSerialiser? EventSerialiser;
    public WebhookEventOptions? EventOptions;
}