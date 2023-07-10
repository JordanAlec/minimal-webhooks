using MinimalWebHooks.Core.Models.DbSets;

namespace MinimalWebHooks.Tests.Builders;

public class MockWebhookClientHttpClientBuilder
{
    private readonly Mock<IWebhookClientHttpClient> _client;

    public MockWebhookClientHttpClientBuilder() => _client = new Mock<IWebhookClientHttpClient>();
    public Mock<IWebhookClientHttpClient> Build() => _client;

    public MockWebhookClientHttpClientBuilder Setup(WebhookActionEvent webhookActionEvent, WebhookClient client, bool success)
    {
        _client.Setup(x => x.VerifyWebhookUrl(client)).ReturnsAsync(success);
        var webhookActionEventResult = success ? 
            new WebhookActionEventResult().SuccessfulResult(webhookActionEvent, client) :
            new WebhookActionEventResult().FailedResult(webhookActionEvent, client);

        _client.Setup(x => x.SendEventToWebhookUrl(webhookActionEvent, client)).ReturnsAsync(webhookActionEventResult);
        return this;
    }
}