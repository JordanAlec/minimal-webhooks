namespace MinimalWebHooks.Tests.Builders;

public class MockMinimalWebhookOptionsProcessorBuilder
{
    private readonly Mock<IMinimalWebhookOptionsProcessor> _processor;

    public MockMinimalWebhookOptionsProcessorBuilder() => _processor = new Mock<IMinimalWebhookOptionsProcessor>();

    public Mock<IMinimalWebhookOptionsProcessor> Build() => _processor;

    public MockMinimalWebhookOptionsProcessorBuilder SetupVerifyWebhookUrl(WebhookClient client, bool returnsSuccessful)
    {
        _processor.Setup(x => x.VerifyWebhookUrl(client))
            .ReturnsAsync(returnsSuccessful);

        return this;
    }
}