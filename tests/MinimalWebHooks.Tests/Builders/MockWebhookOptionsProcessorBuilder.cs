using MinimalWebHooks.Core.Models.DbSets;

namespace MinimalWebHooks.Tests.Builders;

public class MockWebhookOptionsProcessorBuilder
{
    private readonly Mock<IWebhookOptionsProcessor> _processor;

    public MockWebhookOptionsProcessorBuilder() => _processor = new Mock<IWebhookOptionsProcessor>();

    public Mock<IWebhookOptionsProcessor> Build() => _processor;

    public MockWebhookOptionsProcessorBuilder SetupVerifyWebhookUrl(WebhookClient client, bool returnsSuccessful)
    {
        _processor.Setup(x => x.VerifyWebhookUrl(client))
            .ReturnsAsync(returnsSuccessful);

        return this;
    }
}