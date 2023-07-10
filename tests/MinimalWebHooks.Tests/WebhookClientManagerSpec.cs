using MinimalWebHooks.Core.Enum;

namespace MinimalWebHooks.Tests
{
    public class WebhookClientManagerSpec
    {
        public class CanGetClients : WebhookClientManagerBaseSpec, IAsyncLifetime
        {
            public CanGetClients() : base(
                new MockWebhookDataStoreBuilder().SetupClients(FakeData.WebhookClients(1)),
                new MockWebhookOptionsProcessorBuilder())
            { }

            [Fact]
            public void DataStoreCanGetClients()
            {
                DataStore.Verify(x => x.Get(), Times.Once);
                DataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultIsSuccessful() => Result.Success.Should().BeTrue();

            [Fact]
            public void ResultMessage() => Result.Message.Should().Contain("Clients found.");

            public async Task InitializeAsync() => Result = await Manager.Get();

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CanGetClientsByEntity : WebhookClientManagerBaseSpec, IAsyncLifetime
        {
            private static readonly List<WebhookClient> WebhookClients = FakeData.WebhookClients(1);
            public CanGetClientsByEntity() : base(
                new MockWebhookDataStoreBuilder().SetupClients(WebhookClients).SetupClientsGetByEntity(WebhookClients),
                new MockWebhookOptionsProcessorBuilder())
            { }

            [Fact]
            public void DataStoreCanGetClients()
            {
                DataStore.Verify(x => x.GetByEntity(It.IsAny<WebhookClient>(), WebhookActionType.Create), Times.Once);
                DataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultIsSuccessful() => Result.Success.Should().BeTrue();

            [Fact]
            public void ResultMessage() => Result.Message.Should().Contain("Clients found.");

            public async Task InitializeAsync() => Result = await Manager.GetByEntity(new WebhookClient(), WebhookActionType.Create);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CannotFindClients : WebhookClientManagerBaseSpec, IAsyncLifetime
        {
            public CannotFindClients() : base(new MockWebhookDataStoreBuilder(), new MockWebhookOptionsProcessorBuilder())
            { }

            [Fact]
            public void DataStoreCanGetClients()
            {
                DataStore.Verify(x => x.Get(), Times.Once);
                DataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultIsSuccessful() => Result.Success.Should().BeFalse();

            [Fact]
            public void ResultMessage() => Result.Message.Should().Contain("No clients found.");

            public async Task InitializeAsync() => Result = await Manager.Get();

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CannotFindClientsByEntity : WebhookClientManagerBaseSpec, IAsyncLifetime
        {
            public CannotFindClientsByEntity() : base(new MockWebhookDataStoreBuilder(), new MockWebhookOptionsProcessorBuilder())
            { }

            [Fact]
            public void DataStoreCanGetClients()
            {
                DataStore.Verify(x => x.GetByEntity(It.IsAny<WebhookClient>(), WebhookActionType.Create), Times.Once);
                DataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultIsSuccessful() => Result.Success.Should().BeFalse();

            [Fact]
            public void ResultMessage() => Result.Message.Should().Contain("No clients found.");

            public async Task InitializeAsync() => Result = await Manager.GetByEntity(new WebhookClient(), WebhookActionType.Create);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CanGetClientById : WebhookClientManagerBaseSpec, IAsyncLifetime
        {
            private static readonly WebhookClient Client = FakeData.WebhookClient();
            public CanGetClientById() : base(
                new MockWebhookDataStoreBuilder().SetupClient(Client),
                new MockWebhookOptionsProcessorBuilder())
            { }

            [Fact]
            public void DataStoreCanGetClient()
            {
                DataStore.Verify(x => x.GetById(Client.Id), Times.Once);
                DataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultIsSuccessful() => Result.Success.Should().BeTrue();

            [Fact]
            public void ResultMessage() => Result.Message.Should().Contain($"Client found with Id: {Client.Id}.");

            public async Task InitializeAsync() => Result = await Manager.Get(Client.Id);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CannotFindClientById : WebhookClientManagerBaseSpec, IAsyncLifetime
        {
            private static readonly int Id = 1;

            public CannotFindClientById() : base(new MockWebhookDataStoreBuilder(), new MockWebhookOptionsProcessorBuilder())
            { }

            [Fact]
            public void DataStoreCanGetClient()
            {
                DataStore.Verify(x => x.GetById(Id), Times.Once);
                DataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultIsNotSuccessful() => Result.Success.Should().BeFalse();

            [Fact]
            public void ResultMessage() => Result.Message.Should().Contain("Client not found with Id: 1.");

            public async Task InitializeAsync() => Result = await Manager.Get(Id);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CanSaveClient : WebhookClientManagerBaseSpec, IAsyncLifetime
        {
            private static readonly WebhookClient Client = FakeData.WebhookClient();
            public CanSaveClient() : base(
                new MockWebhookDataStoreBuilder().SetupCreateClient(Client),
                new MockWebhookOptionsProcessorBuilder().SetupVerifyWebhookUrl(Client, true))
            { }

            [Fact]
            public void DataStoreCanGetAndCreateClient()
            {
                DataStore.Verify(x => x.GetByName(Client.Name), Times.Once);
                DataStore.Verify(x => x.Create(Client), Times.Once);
                DataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultIsSuccessful() => Result.Success.Should().BeTrue();

            [Fact]
            public void ResultMessage() => Result.Message.Should().Contain("Successfully created client.");

            public async Task InitializeAsync() => Result = await Manager.Create(Client);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CannotSaveKnownClient : WebhookClientManagerBaseSpec, IAsyncLifetime
        {
            private static readonly WebhookClient Client = FakeData.WebhookClient();
            public CannotSaveKnownClient() : base(
                new MockWebhookDataStoreBuilder().SetupClient(Client).SetupCreateClient(Client),
                new MockWebhookOptionsProcessorBuilder().SetupVerifyWebhookUrl(Client, true))
            { }

            [Fact]
            public void DataStoreCanGetButNotCreateClient()
            {
                DataStore.Verify(x => x.GetByName(Client.Name), Times.Once);
                DataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultIsNotSuccessful() => Result.Success.Should().BeFalse();

            [Fact]
            public void ResultMessage() => Result.Message.Should().Contain("Client already exists with this name.");

            public async Task InitializeAsync() => Result = await Manager.Create(Client);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CannotSaveClientWithNoVerifibleWebhookUrl : WebhookClientManagerBaseSpec, IAsyncLifetime
        {
            private static readonly WebhookClient Client = FakeData.WebhookClient();
            public CannotSaveClientWithNoVerifibleWebhookUrl() : base(
                new MockWebhookDataStoreBuilder(),
                new MockWebhookOptionsProcessorBuilder().SetupVerifyWebhookUrl(Client, false))
            { }

            [Fact]
            public void ClientWebhookUrlIsCheck() => OptionsProcessor.Verify(x => x.VerifyWebhookUrl(Client), Times.Once);

            [Fact]
            public void DataStoreIsNotChecked() => DataStore.VerifyNoOtherCalls();

            [Fact]
            public void ResultIsNotSuccessful() => Result.Success.Should().BeFalse();

            [Fact]
            public void ResultMessage() => Result.Message.Should().Contain("Cannot verify client 'WebhookUrl'. Make sure the URL can receive a HEAD request or do not set 'WebhookUrlIsReachable'.");

            public async Task InitializeAsync() => Result = await Manager.Create(Client);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CanDisableClient : WebhookClientManagerBaseSpec, IAsyncLifetime
        {
            private static readonly WebhookClient Client = FakeData.WebhookClient();
            public CanDisableClient() : base(
                new MockWebhookDataStoreBuilder().SetupClient(Client).SetupDisableClient(Client),
                new MockWebhookOptionsProcessorBuilder().SetupVerifyWebhookUrl(Client, true))
            { }

            [Fact]
            public void DataStoreCanGetAndDisableClient()
            {
                DataStore.Verify(x => x.GetById(Client.Id), Times.Once);
                DataStore.Verify(x => x.Disable(Client), Times.Once);
                DataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultIsSuccessful() => Result.Success.Should().BeTrue();

            [Fact]
            public void ResultMessage() => Result.Message.Should().Contain($"Client disabled with Id: {Client.Id}.");

            public async Task InitializeAsync() => Result = await Manager.Disable(Client.Id);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CannotDisableClient : WebhookClientManagerBaseSpec, IAsyncLifetime
        {
            private static readonly WebhookClient Client = FakeData.WebhookClient();
            public CannotDisableClient() : base(
                new MockWebhookDataStoreBuilder().SetupDisableClient(Client),
                new MockWebhookOptionsProcessorBuilder().SetupVerifyWebhookUrl(Client, true))
            { }

            [Fact]
            public void DataStoreCanGetAndDisableClient()
            {
                DataStore.Verify(x => x.GetById(Client.Id), Times.Once);
                DataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultIsSuccessful() => Result.Success.Should().BeFalse();

            [Fact]
            public void ResultMessage() => Result.Message.Should().Contain($"Client not found with Id: {Client.Id}.");

            public async Task InitializeAsync() => Result = await Manager.Disable(Client.Id);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }
    }
}