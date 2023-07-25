using System.Text;
using Microsoft.Extensions.Logging;
using MinimalWebHooks.Core.Interfaces;
using MinimalWebHooks.Core.Models;
using MinimalWebHooks.Core.Models.DbSets;
using MinimalWebHooks.Core.Serialisation;

namespace MinimalWebHooks.Core.Http;

public class WebhookClientHttpClient : IWebhookClientHttpClient
{
    private readonly ILogger<WebhookClientHttpClient> _logger;
    private readonly IWebhookActionEventSerialiser _eventSerialiser;
    private readonly WebhookOptions _options;
    public WebhookClientHttpClient(ILogger<WebhookClientHttpClient> logger, WebhookOptions options)
    {
        _logger = logger;
        _options = options;
        _eventSerialiser = options.EventSerialiser ?? new DefaultWebhookActionEventSerialiser();
    }

    public async Task<WebhookActionEventResult> SendEventToWebhookUrl(WebhookActionEvent webhookActionEvent, WebhookClient webhookClient)
    {
        var client = HttpClientWithWebhookFactory.Create(webhookClient);
        try
        {
            var response = await client.PostAsync(webhookClient.WebhookUrl,
                new StringContent(_eventSerialiser.SerialiseEvent(webhookActionEvent), Encoding.UTF8, _eventSerialiser.GetMediaType()));
            var content = await response.Content.ReadAsStringAsync();

            return response.IsSuccessStatusCode ? 
                new WebhookActionEventResult().SuccessfulResult(webhookActionEvent, webhookClient, (int)response.StatusCode, content) : 
                new WebhookActionEventResult().FailedResult(webhookActionEvent, webhookClient, (int)response.StatusCode, content);
        }
        catch (Exception ex)
        {
            return new WebhookActionEventResult().FailedResult(webhookActionEvent, webhookClient, 500, ex.Message);
        }
    }

    public async Task<bool> VerifyWebhookUrl(WebhookClient client)
    {
        if (!_options.VerifyWebhookUrl) return true;
        if (string.IsNullOrWhiteSpace(client.WebhookUrl)) return false;
        try
        {
            var httpClient = new HttpClient();
            var result = await httpClient.SendAsync(new HttpRequestMessage
            {
                RequestUri = new Uri(client.WebhookUrl),
                Method = HttpMethod.Head
            });

            _logger.LogDebug("{logger}: Verifying client ({clientName}) webhook url: {url}. Success: {success}", nameof(WebhookClientHttpClient), client.Name, client.WebhookUrl, result.IsSuccessStatusCode);

            return result.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{logger}: Error trying to verify webhook url ({clientName}): {url}", nameof(WebhookClientHttpClient), client.Name, client.WebhookUrl);
            return false;
        }
    }
}