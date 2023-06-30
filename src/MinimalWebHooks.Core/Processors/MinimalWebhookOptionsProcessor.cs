using MinimalWebHooks.Core.Interfaces;
using MinimalWebHooks.Core.Models;

namespace MinimalWebHooks.Core.Processors;

public class MinimalWebhookOptionsProcessor : IMinimalWebhookOptionsProcessor
{
    private readonly MinimalWebhookOptions _options;
    public MinimalWebhookOptionsProcessor(MinimalWebhookOptions options) => _options = options;

    public async Task<bool> VerifyWebhookUrl(WebhookClient client)
    {
        if (!_options.VerifyWebhookUrl) return true;
        if (string.IsNullOrWhiteSpace(client.WebhookUrl)) return false;
        try
        {
            var httpClient = new HttpClient { BaseAddress = new Uri(client.WebhookUrl) };
            var result = await httpClient.SendAsync(new HttpRequestMessage
            {
                Method = HttpMethod.Head
            });

            return result.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}