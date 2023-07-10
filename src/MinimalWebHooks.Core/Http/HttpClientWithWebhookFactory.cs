using MinimalWebHooks.Core.Models;

namespace MinimalWebHooks.Core.Http;

public static class HttpClientWithWebhookFactory
{
    public static HttpClient Create(WebhookClient webhookClient)
    {
        var client = new HttpClient();
        if (webhookClient.Headers != null && webhookClient.Headers.Any())
            foreach (var header in webhookClient.Headers)
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
        
        return client;
    }
}