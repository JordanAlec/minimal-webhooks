using MinimalWebHooks.Core.Models.DbSets;

namespace MinimalWebHooks.Tests.Builders;

public class MockWebhookClientHttpClientBuilder
{
    private readonly Mock<IWebhookClientHttpClient> _client;

    public MockWebhookClientHttpClientBuilder() => _client = new Mock<IWebhookClientHttpClient>();
    public Mock<IWebhookClientHttpClient> Build() => _client;

    public MockWebhookClientHttpClientBuilder Setup(WebhookActionEvent webhookActionEvent, WebhookClient client, bool success)
    {
        var statusCode = success ? 200 : 500;
        _client.Setup(x => x.VerifyWebhookUrl(client)).ReturnsAsync(success);
        var webhookActionEventResult = success ? 
            new WebhookActionEventResult().SuccessfulResult(webhookActionEvent, client, statusCode) :
            new WebhookActionEventResult().FailedResult(webhookActionEvent, client, statusCode);

        _client.Setup(x => x.SendEventToWebhookUrl(webhookActionEvent, client)).ReturnsAsync(webhookActionEventResult);
        return this;
    }

    public MockWebhookClientHttpClientBuilder SetupVerify(WebhookClient client, bool success)
    {
        _client.Setup(x => x.VerifyWebhookUrl(client)).ReturnsAsync(success);
        return this;
    }
}