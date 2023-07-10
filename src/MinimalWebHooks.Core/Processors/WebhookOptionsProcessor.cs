using MinimalWebHooks.Core.Interfaces;
using MinimalWebHooks.Core.Models;
using MinimalWebHooks.Core.Models.DbSets;

namespace MinimalWebHooks.Core.Processors;

public class WebhookOptionsProcessor : IWebhookOptionsProcessor
{
    private readonly WebhookOptions _options;
    private readonly IWebhookClientHttpClient _webhookHttpClient;
    public WebhookOptionsProcessor(WebhookOptions options, IWebhookClientHttpClient webhookHttpClient)
    {
        _options = options;
        _webhookHttpClient = webhookHttpClient;
    }

    public async Task<bool> VerifyWebhookUrl(WebhookClient client)
    {
        if (!_options.VerifyWebhookUrl) return true;
        return await _webhookHttpClient.VerifyWebhookUrl(client);
    }
}