using System.Text;
using MinimalWebHooks.Core.Interfaces;
using MinimalWebHooks.Core.Models;
using MinimalWebHooks.Core.Serialisation;

namespace MinimalWebHooks.Core.Http;

public class WebhookClientHttpClient : IWebhookClientHttpClient
{
    private readonly IWebhookActionEventSerialiser _eventSerialiser;
    public WebhookClientHttpClient(WebhookOptions options)
    {
        _eventSerialiser = options.EventSerialiser ?? new DefaultWebhookActionEventSerialiser();
    }

    public async Task<WebhookActionEventResult> SendEventToWebhookUrl(WebhookActionEvent webhookActionEvent, WebhookClient webhookClient)
    {
        var client = new HttpClient();
        try
        {
            var response = await client.PostAsync(webhookClient.WebhookUrl,
                new StringContent(_eventSerialiser.SerialiseEvent(webhookActionEvent), Encoding.UTF8, _eventSerialiser.GetMediaType()));
            var content = await response.Content.ReadAsStringAsync();

            return response.IsSuccessStatusCode ? 
                new WebhookActionEventResult().SuccessfulResult(webhookActionEvent, webhookClient, content) : 
                new WebhookActionEventResult().FailedResult(webhookActionEvent, webhookClient, content);
        }
        catch (Exception ex)
        {
            return new WebhookActionEventResult().FailedResult(webhookActionEvent, webhookClient, ex.Message);
        }
        
    }

    public async Task<bool> VerifyWebhookUrl(WebhookClient client)
    {
        if (string.IsNullOrWhiteSpace(client.WebhookUrl)) return false;
        try
        {
            var httpClient = new HttpClient();
            var result = await httpClient.SendAsync(new HttpRequestMessage
            {
                RequestUri = new Uri(client.WebhookUrl),
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