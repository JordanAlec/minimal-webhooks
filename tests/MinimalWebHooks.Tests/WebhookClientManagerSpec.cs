using Microsoft.Extensions.Logging;
using MinimalWebHooks.Core.Enum;
using MinimalWebHooks.Core.Models.DbSets;

namespace MinimalWebHooks.Tests
{
    public class WebhookClientManagerSpec
    {
        public class CanGetClients : IAsyncLifetime
        {
            private readonly Mock<IWebhookDataStore> _dataStore;
            private readonly WebhookClientManager _manager;
            private WebhookDataResult _result;

            public CanGetClients()
            {
                _dataStore = new MockWebhookDataStoreBuilder().SetupClients(FakeData.WebhookClients(1)).Build();
                var httpClient = new MockWebhookClientHttpClientBuilder().Build();
                _manager = new WebhookClientManager(new Mock<ILogger<WebhookClientManager>>().Object, _dataStore.Object, httpClient.Object);
            }

            [Fact]
            public void DataStoreCanGetClients()
            {
                _dataStore.Verify(x => x.Get(), Times.Once);
                _dataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultSuccess() => _result.Success.Should().BeTrue();

            [Fact]
            public void ResultMessage() => _result.Message.Should().Contain("Clients found.");

            public async Task InitializeAsync() => _result = await _manager.Get();

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CanGetClientsByEntity : IAsyncLifetime
        {
            private readonly Mock<IWebhookDataStore> _dataStore;
            private readonly WebhookClientManager _manager;
            private WebhookDataResult _result;
            private readonly List<WebhookClient> _webhookClients;

            public CanGetClientsByEntity()
            {
                _webhookClients = FakeData.WebhookClients(1);

                _dataStore = new MockWebhookDataStoreBuilder().SetupClients(_webhookClients).SetupClientsGetByEntity(_webhookClients).Build();
                var httpClient = new MockWebhookClientHttpClientBuilder().Build();
                _manager = new WebhookClientManager(new Mock<ILogger<WebhookClientManager>>().Object, _dataStore.Object, httpClient.Object);
            }


            [Fact]
            public void DataStoreCanGetClients()
            {
                _dataStore.Verify(x => x.GetByEntity(It.IsAny<WebhookClient>(), WebhookActionType.Create), Times.Once);
                _dataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultSuccess() => _result.Success.Should().BeTrue();

            [Fact]
            public void ResultMessage() => _result.Message.Should().Contain("Clients found.");

            public async Task InitializeAsync() => _result = await _manager.GetByEntity(new WebhookClient(), WebhookActionType.Create);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CannotFindClients : IAsyncLifetime
        {
            private readonly Mock<IWebhookDataStore> _dataStore;
            private readonly WebhookClientManager _manager;
            private WebhookDataResult _result;

            public CannotFindClients()
            {
                _dataStore = new MockWebhookDataStoreBuilder().Build();
                var httpClient = new MockWebhookClientHttpClientBuilder().Build();
                _manager = new WebhookClientManager(new Mock<ILogger<WebhookClientManager>>().Object, _dataStore.Object, httpClient.Object);
            }

            [Fact]
            public void DataStoreCanGetClients()
            {
                _dataStore.Verify(x => x.Get(), Times.Once);
                _dataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultSuccess() => _result.Success.Should().BeFalse();

            [Fact]
            public void ResultMessage() => _result.Message.Should().Contain("No clients found.");

            public async Task InitializeAsync() => _result = await _manager.Get();

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CannotFindClientsByEntity : IAsyncLifetime
        {
            private readonly Mock<IWebhookDataStore> _dataStore;
            private readonly WebhookClientManager _manager;
            private WebhookDataResult _result;

            public CannotFindClientsByEntity()
            {
                _dataStore = new MockWebhookDataStoreBuilder().Build();
                var httpClient = new MockWebhookClientHttpClientBuilder().Build();
                _manager = new WebhookClientManager(new Mock<ILogger<WebhookClientManager>>().Object, _dataStore.Object, httpClient.Object);
            }

            [Fact]
            public void DataStoreCanGetClients()
            {
                _dataStore.Verify(x => x.GetByEntity(It.IsAny<WebhookClient>(), WebhookActionType.Create), Times.Once);
                _dataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultSuccess() => _result.Success.Should().BeFalse();

            [Fact]
            public void ResultMessage() => _result.Message.Should().Contain("No clients found.");

            public async Task InitializeAsync() => _result = await _manager.GetByEntity(new WebhookClient(), WebhookActionType.Create);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CanGetClientById : IAsyncLifetime
        {
            private readonly Mock<IWebhookDataStore> _dataStore;
            private readonly WebhookClientManager _manager;
            private WebhookDataResult _result;

            private readonly WebhookClient _client;

            public CanGetClientById()
            {
                _client = FakeData.WebhookClient();
                _dataStore = new MockWebhookDataStoreBuilder().SetupClient(_client).Build();
                var httpClient = new MockWebhookClientHttpClientBuilder().Build();
                _manager = new WebhookClientManager(new Mock<ILogger<WebhookClientManager>>().Object, _dataStore.Object, httpClient.Object);
            }

            [Fact]
            public void DataStoreCanGetClient()
            {
                _dataStore.Verify(x => x.GetById(_client.Id, true, 6), Times.Once);
                _dataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultSuccess() => _result.Success.Should().BeTrue();

            [Fact]
            public void ResultMessage() => _result.Message.Should().Contain($"Client found with Id: {_client.Id}.");

            public async Task InitializeAsync() => _result = await _manager.Get(_client.Id);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CannotFindClientById : IAsyncLifetime
        {
            private readonly Mock<IWebhookDataStore> _dataStore;
            private readonly WebhookClientManager _manager;
            private WebhookDataResult _result;
            private static readonly int Id = 1;

            public CannotFindClientById()
            {
                _dataStore = new MockWebhookDataStoreBuilder().Build();
                var httpClient = new MockWebhookClientHttpClientBuilder().Build();
                _manager = new WebhookClientManager(new Mock<ILogger<WebhookClientManager>>().Object, _dataStore.Object, httpClient.Object);
            }

            [Fact]
            public void DataStoreCanGetClient()
            {
                _dataStore.Verify(x => x.GetById(Id, true, 6), Times.Once);
                _dataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultSuccess() => _result.Success.Should().BeFalse();

            [Fact]
            public void ResultMessage() => _result.Message.Should().Contain("Client not found with Id: 1.");

            public async Task InitializeAsync() => _result = await _manager.Get(Id);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CanSaveClient : IAsyncLifetime
        {
            private readonly Mock<IWebhookDataStore> _dataStore;
            private readonly WebhookClientManager _manager;
            private WebhookDataResult _result;

            private readonly WebhookClient _client;

            public CanSaveClient()
            {
                _client = FakeData.WebhookClient();
                _dataStore = new MockWebhookDataStoreBuilder().SetupCreateClient(_client).Build();
                var httpClient = new MockWebhookClientHttpClientBuilder().SetupVerify(_client, true).Build();
                _manager = new WebhookClientManager(new Mock<ILogger<WebhookClientManager>>().Object, _dataStore.Object, httpClient.Object);
            }

            [Fact]
            public void DataStoreCanGetAndCreateClient()
            {
                _dataStore.Verify(x => x.GetByName(_client.Name));
                _dataStore.Verify(x => x.GetById(_client.Id, false, 6));
                _dataStore.Verify(x => x.Create(_client), Times.Once);
                _dataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultSuccess() => _result.Success.Should().BeTrue();

            [Fact]
            public void ResultMessage() => _result.Message.Should().Contain("Successfully created client.");

            public async Task InitializeAsync() => _result = await _manager.Create(_client);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CannotSaveKnownClient : IAsyncLifetime
        {
            private readonly Mock<IWebhookDataStore> _dataStore;
            private readonly WebhookClientManager _manager;
            private WebhookDataResult _result;

            private readonly WebhookClient _client;

            public CannotSaveKnownClient()
            {
                _client = FakeData.WebhookClient();
                _dataStore = new MockWebhookDataStoreBuilder().SetupClient(_client).SetupCreateClient(_client).Build();
                var httpClient = new MockWebhookClientHttpClientBuilder().SetupVerify(_client, true).Build();
                _manager = new WebhookClientManager(new Mock<ILogger<WebhookClientManager>>().Object, _dataStore.Object, httpClient.Object);
            }

            [Fact]
            public void DataStoreCanGetButNotCreateClient()
            {
                _dataStore.Verify(x => x.GetByName(_client.Name), Times.Once);
                _dataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultSuccess() => _result.Success.Should().BeFalse();

            [Fact]
            public void ResultMessage() => _result.Message.Should().Contain("Client already exists with this name.");

            public async Task InitializeAsync() => _result = await _manager.Create(_client);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CannotSaveClientWithNoVerifibleWebhookUrl : IAsyncLifetime
        {
            private readonly Mock<IWebhookDataStore> _dataStore;
            private readonly Mock<IWebhookClientHttpClient> _httpClient;
            private readonly WebhookClientManager _manager;
            private WebhookDataResult _result;

            private readonly WebhookClient _client;

            public CannotSaveClientWithNoVerifibleWebhookUrl()
            {
                _client = FakeData.WebhookClient();
                _dataStore = new MockWebhookDataStoreBuilder().Build();
                _httpClient = new MockWebhookClientHttpClientBuilder().SetupVerify(_client, false).Build();
                _manager = new WebhookClientManager(new Mock<ILogger<WebhookClientManager>>().Object, _dataStore.Object, _httpClient.Object);
            }

            [Fact]
            public void ClientWebhookUrlIsCheck() => _httpClient.Verify(x => x.VerifyWebhookUrl(_client), Times.Once);

            [Fact]
            public void DataStoreIsNotChecked() => _dataStore.VerifyNoOtherCalls();

            [Fact]
            public void ResultSuccess() => _result.Success.Should().BeFalse();

            [Fact]
            public void ResultMessage() => _result.Message.Should().Contain("Cannot verify client 'WebhookUrl'. Make sure the URL can receive a HEAD request or do not set 'WebhookUrlIsReachable'.");

            public async Task InitializeAsync() => _result = await _manager.Create(_client);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CanDisableClient : IAsyncLifetime
        {
            private readonly Mock<IWebhookDataStore> _dataStore;
            private readonly WebhookClientManager _manager;
            private WebhookDataResult _result;

            private readonly WebhookClient _client;

            public CanDisableClient()
            {
                _client = FakeData.WebhookClient();
                _dataStore = new MockWebhookDataStoreBuilder().SetupClient(_client).SetupUpdateClient(_client).Build();
                var httpClient = new MockWebhookClientHttpClientBuilder().Build();
                _manager = new WebhookClientManager(new Mock<ILogger<WebhookClientManager>>().Object, _dataStore.Object, httpClient.Object);
            }

            [Fact]
            public void DataStoreCanGetAndDisableClient()
            {
                _dataStore.Verify(x => x.GetById(_client.Id, It.IsAny<bool>(), 6));
                _dataStore.Verify(x => x.Update(_client), Times.Once);
                _dataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultSuccess() => _result.Success.Should().BeTrue();

            [Fact]
            public void ResultMessage() => _result.Message.Should().Contain($"Client disabled with Id: {_client.Id}.");

            [Fact]
            public void ClientDisabled() => _result.Data.First().Disabled.Should().BeTrue();

            public async Task InitializeAsync() => _result = await _manager.Disable(_client.Id);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CannotDisableClient : IAsyncLifetime
        {
            private readonly Mock<IWebhookDataStore> _dataStore;
            private readonly WebhookClientManager _manager;
            private WebhookDataResult _result;

            private readonly WebhookClient _client;

            public CannotDisableClient()
            {
                _client = FakeData.WebhookClient();
                _dataStore = new MockWebhookDataStoreBuilder().SetupUpdateClient(_client).Build();
                var httpClient = new MockWebhookClientHttpClientBuilder().Build();
                _manager = new WebhookClientManager(new Mock<ILogger<WebhookClientManager>>().Object, _dataStore.Object, httpClient.Object);
            }

            [Fact]
            public void DataStoreCanGetOnly()
            {
                _dataStore.Verify(x => x.GetById(_client.Id, true, 6), Times.Once);
                _dataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultSuccess() => _result.Success.Should().BeFalse();

            [Fact]
            public void ResultMessage() => _result.Message.Should().Contain($"Client not found with Id: {_client.Id}.");

            public async Task InitializeAsync() => _result = await _manager.Disable(_client.Id);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CanUpdateClient : IAsyncLifetime
        {
            private readonly Mock<IWebhookDataStore> _dataStore;
            private readonly WebhookClientManager _manager;
            private WebhookDataResult _result;

            private readonly WebhookClient _client;
            private readonly List<WebhookClientHeader> _originalHeaders;
            private readonly WebhookUpdateCommand _command;

            public CanUpdateClient()
            {
                _client = FakeData.WebhookClient();
                _originalHeaders = _client.ClientHeaders!;
                _command = FakeData.WebhookUpdateCommand(_client.Id, true, false, new Dictionary<string, string> { { "Authorization", "Basic dXNlcm5hbWU6cGFzc3dvcmQ=" } });

                _dataStore = new MockWebhookDataStoreBuilder().SetupClient(_client, skipDisabledClients: false).SetupUpdateClient(_client).Build();
                var httpClient = new MockWebhookClientHttpClientBuilder().SetupVerify(_client, true).Build();
                _manager = new WebhookClientManager(new Mock<ILogger<WebhookClientManager>>().Object, _dataStore.Object, httpClient.Object);
            }

            [Fact]
            public void DataStoreCanGetAndUpdateClient()
            {
                _dataStore.Verify(x => x.GetById(_client.Id, false, 6));
                _dataStore.Verify(x => x.Delete(_originalHeaders!), Times.Once);
                _dataStore.Verify(x => x.Update(_client));
                _dataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultSuccess() => _result.Success.Should().BeTrue();

            [Fact]
            public void ResultMessage() => _result.Message.Should().Contain($"Client updated with Id: {_client.Id}.");

            [Fact]
            public void ClientUpdated()
            {
                _result.Data.First().Disabled.Should().BeTrue();
                _result.Data.First().ClientHeaders?.Should().HaveCount(1);
                _result.Data.First().ClientHeaders?.First().Key.Should().Be("Authorization");
                _result.Data.First().ClientHeaders?.First().Value.Should().Be("Basic dXNlcm5hbWU6cGFzc3dvcmQ=");
                _result.Data.First().WebhookUrl.Should().Be(_client.WebhookUrl);
                _originalHeaders.Should().NotBeEquivalentTo(_client.ClientHeaders);
            }

            public async Task InitializeAsync() => _result = await _manager.Update(_command);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CanUpdateClientNoReplacementHeaders : IAsyncLifetime
        {
            private readonly Mock<IWebhookDataStore> _dataStore;
            private readonly WebhookClientManager _manager;
            private WebhookDataResult _result;

            private readonly WebhookClient _client;
            private readonly WebhookUpdateCommand _command;

            public CanUpdateClientNoReplacementHeaders()
            {
                _client = FakeData.WebhookClient();
                _command = FakeData.WebhookUpdateCommand(_client.Id, true, false);
                _dataStore = new MockWebhookDataStoreBuilder().SetupClient(_client, skipDisabledClients: false).SetupUpdateClient(_client).Build();
                var httpClient = new MockWebhookClientHttpClientBuilder().SetupVerify(_client, true).Build();
                _manager = new WebhookClientManager(new Mock<ILogger<WebhookClientManager>>().Object, _dataStore.Object, httpClient.Object);
            }

            [Fact]
            public void DataStoreCanGetAndUpdateClient()
            {
                _dataStore.Verify(x => x.GetById(_client.Id, false, 6));
                _dataStore.Verify(x => x.Update(_client));
                _dataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultSuccess() => _result.Success.Should().BeTrue();

            [Fact]
            public void ResultMessage() => _result.Message.Should().Contain($"Client updated with Id: {_client.Id}.");

            [Fact]
            public void ClientUpdated() => _result.Data.First().Disabled.Should().BeTrue();

            public async Task InitializeAsync() => _result = await _manager.Update(_command);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CanUpdateClientDeleteHeaders : IAsyncLifetime
        {
            private readonly Mock<IWebhookDataStore> _dataStore;
            private readonly WebhookClientManager _manager;
            private WebhookDataResult _result;

            private readonly WebhookClient _client;
            private readonly WebhookUpdateCommand _command;

            public CanUpdateClientDeleteHeaders()
            {
                _client = FakeData.WebhookClient();
                _command = FakeData.WebhookUpdateCommand(_client.Id, true, true);
                _dataStore = new MockWebhookDataStoreBuilder().SetupClient(_client, skipDisabledClients: false).SetupUpdateClient(_client).Build();
                var httpClient = new MockWebhookClientHttpClientBuilder().SetupVerify(_client, true).Build();
                _manager = new WebhookClientManager(new Mock<ILogger<WebhookClientManager>>().Object, _dataStore.Object, httpClient.Object);
            }

            [Fact]
            public void DataStoreCanGetAndUpdateClient()
            {
                _dataStore.Verify(x => x.GetById(_client.Id, false, 6));
                _dataStore.Verify(x => x.Delete(_client.ClientHeaders!), Times.Once);
                _dataStore.Verify(x => x.Update(_client));
                _dataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultSuccess() => _result.Success.Should().BeTrue();

            [Fact]
            public void ResultMessage() => _result.Message.Should().Contain($"Client updated with Id: {_client.Id}.");

            [Fact]
            public void ClientUpdated() => _result.Data.First().Disabled.Should().BeTrue();

            public async Task InitializeAsync() => _result = await _manager.Update(_command);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CannotUpdateClientNotFound : IAsyncLifetime
        {
            private readonly Mock<IWebhookDataStore> _dataStore;
            private readonly WebhookClientManager _manager;
            private WebhookDataResult _result;

            private readonly WebhookClient _client;
            private readonly WebhookUpdateCommand _command;

            public CannotUpdateClientNotFound()
            {
                _client = FakeData.WebhookClient();
                _command = FakeData.WebhookUpdateCommand(_client.Id, true, false, new Dictionary<string, string> { { "Authorization", "Basic dXNlcm5hbWU6cGFzc3dvcmQ=" } });

                _dataStore = new MockWebhookDataStoreBuilder().SetupUpdateClient(_client).Build();
                var httpClient = new MockWebhookClientHttpClientBuilder().Build();
                _manager = new WebhookClientManager(new Mock<ILogger<WebhookClientManager>>().Object, _dataStore.Object, httpClient.Object);
            }

            [Fact]
            public void DataStoreCanGetOnly()
            {
                _dataStore.Verify(x => x.GetById(_client.Id, false, 6), Times.Once);
                _dataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultSuccess() => _result.Success.Should().BeFalse();

            [Fact]
            public void ResultMessage() => _result.Message.Should().Contain($"Client not found with Id: {_client.Id}.");

            public async Task InitializeAsync() => _result = await _manager.Update(_command);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CannotUpdateClientInvalidUrl : IAsyncLifetime
        {
            private readonly Mock<IWebhookDataStore> _dataStore;
            private readonly WebhookClientManager _manager;
            private WebhookDataResult _result;

            private readonly WebhookClient _client;
            private readonly WebhookUpdateCommand _command;

            public CannotUpdateClientInvalidUrl()
            {
                _client = FakeData.WebhookClient();
                _command = FakeData.WebhookUpdateCommand(_client.Id, false, false);

                _dataStore = new MockWebhookDataStoreBuilder().SetupClient(_client, skipDisabledClients: false).SetupUpdateClient(_client).Build();
                var httpClient = new MockWebhookClientHttpClientBuilder().SetupVerify(_client, false).Build();
                _manager = new WebhookClientManager(new Mock<ILogger<WebhookClientManager>>().Object, _dataStore.Object, httpClient.Object);
            }

            [Fact]
            public void DataStoreCanGetAndUpdateClient()
            {
                _dataStore.Verify(x => x.GetById(_client.Id, false, 6), Times.Once);
                _dataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultSuccess() => _result.Success.Should().BeFalse();

            [Fact]
            public void ResultMessage() => _result.Message.Should().Contain("Cannot verify client 'WebhookUrl'. Make sure the URL can receive a HEAD request or do not set 'WebhookUrlIsReachable'.");

            public async Task InitializeAsync() => _result = await _manager.Update(_command);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }
        
        public class CanAddLogToClient : IAsyncLifetime
        {
            private readonly Mock<IWebhookDataStore> _dataStore;
            private readonly WebhookClientManager _manager;
            private WebhookDataResult _result;
            private readonly WebhookClient _client;

            public CanAddLogToClient()
            {
                _client = FakeData.WebhookClient();
                _dataStore = new MockWebhookDataStoreBuilder().SetupClient(_client, skipDisabledClients: false).SetupUpdateClient(_client).Build();
                var httpClient = new MockWebhookClientHttpClientBuilder().Build();
                _manager = new WebhookClientManager(new Mock<ILogger<WebhookClientManager>>().Object, _dataStore.Object, httpClient.Object);
            }

            [Fact]
            public void DataStoreCanGetClient()
            {
                _dataStore.Verify(x => x.GetById(_client.Id, false, 6));
                _dataStore.Verify(x => x.Update(_client), Times.Once);
                _dataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultSuccess() => _result.Success.Should().BeTrue();

            [Fact]
            public void ResultMessage() => _result.Message.Should().Contain($"Log added to client with Id: {_client.Id}.");

            public async Task InitializeAsync() => _result = await _manager.AddLogToClient(_client.Id, new WebhookClientActivityLog().CreateLog(ActivityLogType.CalledWebhookUrl, "Called Webhook Url"));

            public async Task DisposeAsync() => await Task.CompletedTask;
        }
    }
}