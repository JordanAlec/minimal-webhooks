namespace MinimalWebHooks.Tests.Builders;

public class MockWebhookActionEventProcessorBuilder
{
    private readonly Mock<IWebhookActionEventProcessor> _processor;
    private readonly List<WebhookActionEvent> _actionEvents;

    public MockWebhookActionEventProcessorBuilder()
    {
        _processor = new Mock<IWebhookActionEventProcessor>();
        _actionEvents = new List<WebhookActionEvent>();
    }

    public Mock<IWebhookActionEventProcessor> Build() => _processor;
    public List<WebhookActionEvent> GetWrittenEvents() => _actionEvents;

    public MockWebhookActionEventProcessorBuilder Setup(List<WebhookActionEvent> actionEvents, bool success)
    {
        _processor.Setup(x => x.HasEvents()).Returns(success);
        _processor.Setup(x => x.GetEvents()).ReturnsAsync(actionEvents);
        _processor.Setup(x => x.WriteEvent(It.IsAny<WebhookActionEvent>()))
            .Callback<WebhookActionEvent>(actionEvent =>
            {
                _actionEvents.Add(actionEvent);
            });
        return this;
    }
}