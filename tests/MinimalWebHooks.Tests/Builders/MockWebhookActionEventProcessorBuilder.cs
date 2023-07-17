namespace MinimalWebHooks.Tests.Builders;

public class MockWebhookActionEventProcessorBuilder
{
    private readonly Mock<IWebhookActionEventProcessor> _processor;

    public MockWebhookActionEventProcessorBuilder() => _processor = new Mock<IWebhookActionEventProcessor>();

    public Mock<IWebhookActionEventProcessor> Build() => _processor;

    public MockWebhookActionEventProcessorBuilder Setup(List<WebhookActionEvent> actionEvents, bool success)
    {
        _processor.Setup(x => x.HasEvents()).Returns(success);
        _processor.Setup(x => x.GetEvents()).ReturnsAsync(actionEvents);
        return this;
    }
}