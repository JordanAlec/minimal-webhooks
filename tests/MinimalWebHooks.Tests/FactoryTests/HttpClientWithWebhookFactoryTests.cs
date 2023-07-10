using MinimalWebHooks.Core.Http;
using MinimalWebHooks.Core.Models.DbSets;

namespace MinimalWebHooks.Tests.FactoryTests;

public class HttpClientWithWebhookFactoryTests
{
    [Fact]
    public void ClientContainsExpectedHeaders()
    {
        var webhookClient = FakeData.WebhookClient();
        webhookClient.ClientHeaders = new List<WebhookClientHeader>
        {
            new() { Key = "Secret", Value = Guid.NewGuid().ToString() },
            new() { Key = "Authorization", Value = "Basic dXNlcm5hbWU6cGFzc3dvcmQ=" }
        };

        var httpClient = HttpClientWithWebhookFactory.Create(webhookClient);
        httpClient.DefaultRequestHeaders.Should().ContainKeys("Secret", "Authorization");
    }
}