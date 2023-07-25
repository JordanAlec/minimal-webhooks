using Microsoft.Extensions.Logging;
using MinimalWebHooks.Core.Enum;
using MinimalWebHooks.Core.Models.DbSets;

namespace MinimalWebHooks.Tests
{
    public class WebhookClientManagerSpec
    {
        public class CanGetClients : WebhookClientManagerBaseSpec, IAsyncLifetime
        {
            public CanGetClients() : base(
                new MockWebhookDataStoreBuilder().SetupClients(FakeData.WebhookClients(1)),
                new MockWebhookClientHttpClientBuilder())
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
                new MockWebhookClientHttpClientBuilder())
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
            public CannotFindClients() : base(new MockWebhookDataStoreBuilder(), new MockWebhookClientHttpClientBuilder())
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
            public CannotFindClientsByEntity() : base(new MockWebhookDataStoreBuilder(), new MockWebhookClientHttpClientBuilder())
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
                new MockWebhookClientHttpClientBuilder())
            { }

            [Fact]
            public void DataStoreCanGetClient()
            {
                DataStore.Verify(x => x.GetById(Client.Id, true), Times.Once);
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

            public CannotFindClientById() : base(new MockWebhookDataStoreBuilder(), new MockWebhookClientHttpClientBuilder())
            { }

            [Fact]
            public void DataStoreCanGetClient()
            {
                DataStore.Verify(x => x.GetById(Id, true), Times.Once);
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
                new MockWebhookClientHttpClientBuilder().SetupVerify(Client, true))
            { }

            [Fact]
            public void DataStoreCanGetAndCreateClient()
            {
                DataStore.Verify(x => x.GetByName(Client.Name));
                DataStore.Verify(x => x.GetById(Client.Id, false));
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
                new MockWebhookClientHttpClientBuilder().SetupVerify(Client, true))
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
                new MockWebhookClientHttpClientBuilder().SetupVerify(Client, false))
            { } 

            [Fact]
            public void ClientWebhookUrlIsCheck() => HttpClient.Verify(x => x.VerifyWebhookUrl(Client), Times.Once);

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
                new MockWebhookDataStoreBuilder().SetupClient(Client).SetupUpdateClient(Client),
                new MockWebhookClientHttpClientBuilder())
            { }

            [Fact]
            public void DataStoreCanGetAndDisableClient()
            {
                DataStore.Verify(x => x.GetById(Client.Id, It.IsAny<bool>()));
                DataStore.Verify(x => x.Update(Client), Times.Once);
                DataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultIsSuccessful() => Result.Success.Should().BeTrue();

            [Fact]
            public void ResultMessage() => Result.Message.Should().Contain($"Client disabled with Id: {Client.Id}.");

            [Fact]
            public void ClientDisabled() => Result.Data.First().Disabled.Should().BeTrue();

            public async Task InitializeAsync() => Result = await Manager.Disable(Client.Id);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CannotDisableClient : WebhookClientManagerBaseSpec, IAsyncLifetime
        {
            private static readonly WebhookClient Client = FakeData.WebhookClient();
            public CannotDisableClient() : base(
                new MockWebhookDataStoreBuilder().SetupUpdateClient(Client),
                new MockWebhookClientHttpClientBuilder())
            { }

            [Fact]
            public void DataStoreCanGetOnly()
            {
                DataStore.Verify(x => x.GetById(Client.Id, true), Times.Once);
                DataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultIsSuccessful() => Result.Success.Should().BeFalse();

            [Fact]
            public void ResultMessage() => Result.Message.Should().Contain($"Client not found with Id: {Client.Id}.");

            public async Task InitializeAsync() => Result = await Manager.Disable(Client.Id);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CanUpdateClient : IAsyncLifetime
        {
            protected Mock<IWebhookDataStore> DataStore { get; set; }
            protected Mock<IWebhookClientHttpClient> HttpClient { get; set; }
            protected WebhookClientManager Manager { get; set; }
            protected WebhookDataResult Result { get; set; }

            private WebhookClient Client;
            private List<WebhookClientHeader>? OriginalHeaders;
            private WebhookUpdateCommand Command;

            public CanUpdateClient()
            {
                Client = FakeData.WebhookClient();
                OriginalHeaders = Client.ClientHeaders;
                Command = FakeData.WebhookUpdateCommand(Client.Id, true, false, new Dictionary<string, string> { { "Authorization", "Basic dXNlcm5hbWU6cGFzc3dvcmQ=" } });

                DataStore = new MockWebhookDataStoreBuilder().SetupClient(Client, skipDisabledClients: false).SetupUpdateClient(Client).Build();
                HttpClient = new MockWebhookClientHttpClientBuilder().SetupVerify(Client, true).Build();
                Manager = new WebhookClientManager(new Mock<ILogger<WebhookClientManager>>().Object, DataStore.Object, HttpClient.Object);
            }

            [Fact]
            public void DataStoreCanGetAndUpdateClient()
            {
                DataStore.Verify(x => x.GetById(Client.Id, false));
                DataStore.Verify(x => x.Delete(OriginalHeaders!), Times.Once);
                DataStore.Verify(x => x.Update(Client));
                DataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultIsSuccessful() => Result.Success.Should().BeTrue();

            [Fact]
            public void ResultMessage() => Result.Message.Should().Contain($"Client updated with Id: {Client.Id}.");

            [Fact]
            public void ClientUpdated()
            {
                Result.Data.First().Disabled.Should().BeTrue();
                Result.Data.First().ClientHeaders?.Should().HaveCount(1);
                Result.Data.First().ClientHeaders?.First().Key.Should().Be("Authorization");
                Result.Data.First().ClientHeaders?.First().Value.Should().Be("Basic dXNlcm5hbWU6cGFzc3dvcmQ=");
                Result.Data.First().WebhookUrl.Should().Be(Client.WebhookUrl);
                OriginalHeaders.Should().NotBeEquivalentTo(Client.ClientHeaders);
            }

            public async Task InitializeAsync() => Result = await Manager.Update(Command);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CanUpdateClientNoReplacementHeaders : WebhookClientManagerBaseSpec, IAsyncLifetime
        {
            private static readonly WebhookClient Client = FakeData.WebhookClient();
            private static readonly WebhookUpdateCommand Command = FakeData.WebhookUpdateCommand(Client.Id, true, false);

            public CanUpdateClientNoReplacementHeaders() : base(
                new MockWebhookDataStoreBuilder().SetupClient(Client, skipDisabledClients: false).SetupUpdateClient(Client),
                new MockWebhookClientHttpClientBuilder().SetupVerify(Client, true))
            { }

            [Fact]
            public void DataStoreCanGetAndUpdateClient()
            {
                DataStore.Verify(x => x.GetById(Client.Id, false));
                DataStore.Verify(x => x.Update(Client));
                DataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultIsSuccessful() => Result.Success.Should().BeTrue();

            [Fact]
            public void ResultMessage() => Result.Message.Should().Contain($"Client updated with Id: {Client.Id}.");

            [Fact]
            public void ClientUpdated() => Result.Data.First().Disabled.Should().BeTrue();

            public async Task InitializeAsync() => Result = await Manager.Update(Command);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CanUpdateClientDeleteHeaders : WebhookClientManagerBaseSpec, IAsyncLifetime
        {
            private static readonly WebhookClient Client = FakeData.WebhookClient();
            private static readonly WebhookUpdateCommand Command = FakeData.WebhookUpdateCommand(Client.Id, true, true);

            public CanUpdateClientDeleteHeaders() : base(
                new MockWebhookDataStoreBuilder().SetupClient(Client, skipDisabledClients: false).SetupUpdateClient(Client),
                new MockWebhookClientHttpClientBuilder().SetupVerify(Client, true))
            { }

            [Fact]
            public void DataStoreCanGetAndUpdateClient()
            {
                DataStore.Verify(x => x.GetById(Client.Id, false));
                DataStore.Verify(x => x.Delete(Client.ClientHeaders!), Times.Once);
                DataStore.Verify(x => x.Update(Client));
                DataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultIsSuccessful() => Result.Success.Should().BeTrue();

            [Fact]
            public void ResultMessage() => Result.Message.Should().Contain($"Client updated with Id: {Client.Id}.");

            [Fact]
            public void ClientUpdated() => Result.Data.First().Disabled.Should().BeTrue();

            public async Task InitializeAsync() => Result = await Manager.Update(Command);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CannotUpdateClientNotFound : WebhookClientManagerBaseSpec, IAsyncLifetime
        {
            private static readonly WebhookClient Client = FakeData.WebhookClient();
            private static readonly WebhookUpdateCommand Command = FakeData.WebhookUpdateCommand(Client.Id, true, false, new Dictionary<string, string> { { "Authorization", "Basic dXNlcm5hbWU6cGFzc3dvcmQ=" } });

            public CannotUpdateClientNotFound() : base(
                new MockWebhookDataStoreBuilder().SetupUpdateClient(Client),
                new MockWebhookClientHttpClientBuilder())
            { }

            [Fact]
            public void DataStoreCanGetOnly()
            {
                DataStore.Verify(x => x.GetById(Client.Id, false), Times.Once);
                DataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultIsNotSuccessful() => Result.Success.Should().BeFalse();

            [Fact]
            public void ResultMessage() => Result.Message.Should().Contain($"Client not found with Id: {Client.Id}.");

            public async Task InitializeAsync() => Result = await Manager.Update(Command);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CannotUpdateClientInvalidUrl : WebhookClientManagerBaseSpec, IAsyncLifetime
        {
            private static readonly WebhookClient Client = FakeData.WebhookClient();
            private static readonly WebhookUpdateCommand Command = FakeData.WebhookUpdateCommand(Client.Id, false, false);

            public CannotUpdateClientInvalidUrl() : base(
                new MockWebhookDataStoreBuilder().SetupClient(Client, skipDisabledClients: false).SetupUpdateClient(Client),
                new MockWebhookClientHttpClientBuilder().SetupVerify(Client, false))
            { }

            [Fact]
            public void DataStoreCanGetAndUpdateClient()
            {
                DataStore.Verify(x => x.GetById(Client.Id, false), Times.Once);
                DataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultIsNotSuccessful() => Result.Success.Should().BeFalse();

            [Fact]
            public void ResultMessage() => Result.Message.Should().Contain("Cannot verify client 'WebhookUrl'. Make sure the URL can receive a HEAD request or do not set 'WebhookUrlIsReachable'.");

            public async Task InitializeAsync() => Result = await Manager.Update(Command);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }
        
        public class CanAddLogToClient : WebhookClientManagerBaseSpec, IAsyncLifetime
        {
            private static readonly WebhookClient Client = FakeData.WebhookClient();
            public CanAddLogToClient() : base(
                new MockWebhookDataStoreBuilder().SetupClient(Client, skipDisabledClients: false).SetupUpdateClient(Client),
                new MockWebhookClientHttpClientBuilder())
            { }

            [Fact]
            public void DataStoreCanGetClient()
            {
                DataStore.Verify(x => x.GetById(Client.Id, false));
                DataStore.Verify(x => x.Update(Client), Times.Once);
                DataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultIsSuccessful() => Result.Success.Should().BeTrue();

            [Fact]
            public void ResultMessage() => Result.Message.Should().Contain($"Log added to client with Id: {Client.Id}.");

            public async Task InitializeAsync() => Result = await Manager.AddLogToClient(Client.Id, new WebhookClientActivityLog().CreateLog(ActivityLogType.CalledWebhookUrl, "Called Webhook Url"));

            public async Task DisposeAsync() => await Task.CompletedTask;
        }
    }
}