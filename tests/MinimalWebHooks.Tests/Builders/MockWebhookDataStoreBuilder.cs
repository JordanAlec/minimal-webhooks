namespace MinimalWebHooks.Tests.Builders;

public class MockWebhookDataStoreBuilder
{
    private Mock<IWebhookDataStore> _dataStore;

    public MockWebhookDataStoreBuilder() => _dataStore = new Mock<IWebhookDataStore>();

    public Mock<IWebhookDataStore> Build() => _dataStore;

    public MockWebhookDataStoreBuilder SetupClients(List<WebhookClient> clients)
    {
        _dataStore.Setup(x => x.Get())
            .ReturnsAsync(clients);
        return this;
    }

    public MockWebhookDataStoreBuilder SetupClient(WebhookClient client)
    {
        _dataStore.Setup(x => x.GetById(client.Id))
            .ReturnsAsync(client);
        _dataStore.Setup(x => x.GetByName(client.Name))
            .ReturnsAsync(client);
        return this;
    }

    public MockWebhookDataStoreBuilder SetupCreateClient(WebhookClient client)
    {
        _dataStore.Setup(x => x.Create(client))
            .ReturnsAsync(client);
        return this;
    }
}