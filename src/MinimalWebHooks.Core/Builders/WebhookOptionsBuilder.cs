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

    internal WebhookOptions Build() => _options;
}