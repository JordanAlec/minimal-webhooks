using MinimalWebHooks.Core.Models.DbSets;

namespace MinimalWebHooks.Core.Http;

public static class HttpClientWithWebhookFactory
{
    public static HttpClient Create(WebhookClient webhookClient)
    {
        var client = new HttpClient();
        if (webhookClient.HasHeaders())
            foreach (var header in webhookClient.ClientHeaders)
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
        
        return client;
    }
}