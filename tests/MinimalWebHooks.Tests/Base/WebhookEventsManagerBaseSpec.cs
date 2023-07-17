using Microsoft.Extensions.Logging;

namespace MinimalWebHooks.Tests.Base;

public class WebhookEventsManagerBaseSpec
{
    protected Mock<IWebhookDataStore> DataStore { get; set; }
    protected Mock<IWebhookActionEventProcessor> EventsProcessor { get; set; }
    protected Mock<IWebhookClientHttpClient> WebhookHttpClient { get; set; }
    protected WebhookEventsManager Manager { get; set; }
    protected List<WebhookActionEventResult> EventResults { get; set; }

    public WebhookEventsManagerBaseSpec(MockWebhookDataStoreBuilder dataStoreBuilder, MockWebhookActionEventProcessorBuilder eventProcessorBuilder,  MockWebhookClientHttpClientBuilder httpClientBuilder)
    {
        DataStore = dataStoreBuilder.Build();
        WebhookHttpClient = httpClientBuilder.Build();
        EventsProcessor = eventProcessorBuilder.Build();
        Manager = new WebhookEventsManager(new Mock<ILogger<WebhookEventsManager>>().Object, DataStore.Object, EventsProcessor.Object, WebhookHttpClient.Object);
    }
}