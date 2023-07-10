using MinimalWebHooks.Core.Http;

namespace MinimalWebHooks.Tests.FactoryTests;

public class HttpClientWithWebhookFactoryTests
{
    [Fact]
    public void ClientContainsExpectedHeaders()
    {
        var webhookClient = FakeData.WebhookClient();
        webhookClient.Headers = new Dictionary<string, string>
        {
            {"Secret", Guid.NewGuid().ToString()},
            {"Authorization", "Basic dXNlcm5hbWU6cGFzc3dvcmQ="}
        };

        var httpClient = HttpClientWithWebhookFactory.Create(webhookClient);
        httpClient.DefaultRequestHeaders.Should().ContainKeys("Secret", "Authorization");
    }
}