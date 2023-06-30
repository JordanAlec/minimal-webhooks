namespace MinimalWebHooks.Tests.Base;

public class WebhookClientManagerBaseSpec
{
    protected Mock<IWebhookDataStore> DataStore { get; set; }
    protected WebhookClientManager Manager { get; set; }
    protected WebhookDataResult Result { get; set; }

    public WebhookClientManagerBaseSpec(MockWebhookDataStoreBuilder builder)
    {
        DataStore = builder.Build();
        Manager = new WebhookClientManager(DataStore.Object);
    }
}