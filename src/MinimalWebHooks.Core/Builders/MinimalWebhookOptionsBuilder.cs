using MinimalWebHooks.Core.Models;

namespace MinimalWebHooks.Core.Builders;

public class MinimalWebhookOptionsBuilder
{
    private readonly MinimalWebhookOptions _options;

    public MinimalWebhookOptionsBuilder() => _options = new MinimalWebhookOptions();

    /// <summary>
    /// Will check if the webhook client's webhook url responds to a HEAD request before creating the client.
    /// </summary>
    /// <returns></returns>
    public MinimalWebhookOptionsBuilder WebhookUrlIsReachable()
    {
        _options.VerifyWebhookUrl = true;
        return this;
    }

    internal MinimalWebhookOptions Build() => _options;
}