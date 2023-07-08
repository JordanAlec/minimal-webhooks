using MinimalWebHooks.Core.Interfaces;
using MinimalWebHooks.Core.Models;

namespace MinimalWebHooks.Core.Builders;

public class WebhookOptionsBuilder
{
    private readonly WebhookOptions _options;

    public WebhookOptionsBuilder() => _options = new WebhookOptions();

    /// <summary>
    /// Will check if the webhook client's webhook url responds to a HEAD request before creating the client.
    /// </summary>
    /// <returns></returns>
    public WebhookOptionsBuilder WebhookUrlIsReachable()
    {
        _options.VerifyWebhookUrl = true;
        return this;
    }

    /// <summary>
    /// Set's Serialiser for 'WebhookActionEvent's when sending events to Webhook client urls. Default uses: System.Text.Json.JsonSerializer
    /// </summary>
    /// <returns></returns>
    public WebhookOptionsBuilder SetWebhookActionEventSerialiser(IWebhookActionEventSerialiser eventSerialiser)
    {
        _options.EventSerialiser = eventSerialiser;
        return this;
    }

    internal WebhookOptions Build() => _options;
}