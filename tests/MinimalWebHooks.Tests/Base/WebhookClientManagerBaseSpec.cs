using MinimalWebHooks.Core.Processors;

namespace MinimalWebHooks.Tests.Base;

public class WebhookClientManagerBaseSpec
{
    protected Mock<IWebhookDataStore> DataStore { get; set; }
    protected Mock<IWebhookClientHttpClient> HttpClient { get; set; }
    protected WebhookClientManager Manager { get; set; }
    protected WebhookDataResult Result { get; set; }

    public WebhookClientManagerBaseSpec(MockWebhookDataStoreBuilder dataStoreBuilder, MockWebhookClientHttpClientBuilder httpClientBuilder)
    {
        DataStore = dataStoreBuilder.Build();
        HttpClient = httpClientBuilder.Build();
        Manager = new WebhookClientManager(DataStore.Object, HttpClient.Object);
    }
}