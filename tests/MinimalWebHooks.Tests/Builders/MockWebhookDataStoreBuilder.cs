using MinimalWebHooks.Core.Enum;

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

    public MockWebhookDataStoreBuilder SetupClientsGetByEntity(List<WebhookClient> clients)
    {
        _dataStore.Setup(x => x.GetByEntity(It.IsAny<object>(), It.IsAny<WebhookActionType>()))
            .ReturnsAsync(clients);
        return this;
    }

    public MockWebhookDataStoreBuilder SetupClient(WebhookClient client, bool skipDisabledClients = true)
    {
        _dataStore.Setup(x => x.GetById(client.Id, skipDisabledClients))
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

    public MockWebhookDataStoreBuilder SetupUpdateClient(WebhookClient client)
    {
        _dataStore.Setup(x => x.Update(client))
            .ReturnsAsync(true);
        return this;
    }
}