namespace MinimalWebHooks.Tests
{
    public class WebhookClientManagerSpec
    {
        public class CanGetClients : WebhookClientManagerBaseSpec, IAsyncLifetime
        {
            public CanGetClients() : base(
                new MockWebhookDataStoreBuilder().SetupClients(FakeData.WebhookClients(1)),
                new MockMinimalWebhookOptionsProcessorBuilder())
            { }

            [Fact]
            public void DataStoreCanGetClients()
            {
                DataStore.Verify(x => x.Get(), Times.Once);
                DataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultIsSuccessful() => Result.Success.Should().BeTrue();

            public async Task InitializeAsync() => Result = await Manager.Get();

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CannotFindClients : WebhookClientManagerBaseSpec, IAsyncLifetime
        {
            public CannotFindClients() : base(new MockWebhookDataStoreBuilder(), new MockMinimalWebhookOptionsProcessorBuilder())
            { }

            [Fact]
            public void DataStoreCanGetClients()
            {
                DataStore.Verify(x => x.Get(), Times.Once);
                DataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultIsSuccessful() => Result.Success.Should().BeFalse();

            public async Task InitializeAsync() => Result = await Manager.Get();

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CanGetClientById : WebhookClientManagerBaseSpec, IAsyncLifetime
        {
            private static readonly WebhookClient Client = FakeData.WebhookClient();
            public CanGetClientById() : base(
                new MockWebhookDataStoreBuilder().SetupClient(Client),
                new MockMinimalWebhookOptionsProcessorBuilder())
            { }

            [Fact]
            public void DataStoreCanGetClient()
            {
                DataStore.Verify(x => x.GetById(Client.Id), Times.Once);
                DataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultIsSuccessful() => Result.Success.Should().BeTrue();

            public async Task InitializeAsync() => Result = await Manager.Get(Client.Id);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CannotFindClientById : WebhookClientManagerBaseSpec, IAsyncLifetime
        {
            private static readonly int Id = 1;

            public CannotFindClientById() : base(new MockWebhookDataStoreBuilder(), new MockMinimalWebhookOptionsProcessorBuilder())
            { }

            [Fact]
            public void DataStoreCanGetClient()
            {
                DataStore.Verify(x => x.GetById(Id), Times.Once);
                DataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultIsNotSuccessful() => Result.Success.Should().BeFalse();

            public async Task InitializeAsync() => Result = await Manager.Get(Id);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CanSaveClient : WebhookClientManagerBaseSpec, IAsyncLifetime
        {
            private static readonly WebhookClient Client = FakeData.WebhookClient();
            public CanSaveClient() : base(
                new MockWebhookDataStoreBuilder().SetupCreateClient(Client),
                new MockMinimalWebhookOptionsProcessorBuilder().SetupVerifyWebhookUrl(Client, true))
            { }

            [Fact]
            public void DataStoreCanGetButNotCreateClient()
            {
                DataStore.Verify(x => x.GetByName(Client.Name), Times.Once);
                DataStore.Verify(x => x.Create(Client), Times.Once);
                DataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultIsSuccessful() => Result.Success.Should().BeTrue();

            public async Task InitializeAsync() => Result = await Manager.Create(Client);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CannotSaveKnownClient : WebhookClientManagerBaseSpec, IAsyncLifetime
        {
            private static readonly WebhookClient Client = FakeData.WebhookClient();
            public CannotSaveKnownClient() : base(
                new MockWebhookDataStoreBuilder().SetupClient(Client).SetupCreateClient(Client),
                new MockMinimalWebhookOptionsProcessorBuilder().SetupVerifyWebhookUrl(Client, true))
            { }

            [Fact]
            public void DataStoreCanGetButNotCreateClient()
            {
                DataStore.Verify(x => x.GetByName(Client.Name), Times.Once);
                DataStore.VerifyNoOtherCalls();
            }

            [Fact]
            public void ResultIsNotSuccessful() => Result.Success.Should().BeFalse();

            public async Task InitializeAsync() => Result = await Manager.Create(Client);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }

        public class CannotSaveClientWithNoVerifibleWebhookUrl : WebhookClientManagerBaseSpec, IAsyncLifetime
        {
            private static readonly WebhookClient Client = FakeData.WebhookClient(emptyWebhookUrl: true);
            public CannotSaveClientWithNoVerifibleWebhookUrl() : base(
                new MockWebhookDataStoreBuilder(),
                new MockMinimalWebhookOptionsProcessorBuilder().SetupVerifyWebhookUrl(Client, false))
            { }

            [Fact]
            public void ClientWebhookUrlIsCheck() => OptionsProcessor.Verify(x => x.VerifyWebhookUrl(Client), Times.Once);

            [Fact]
            public void DataStoreIsNotChecked() => DataStore.VerifyNoOtherCalls();

            [Fact]
            public void ResultIsNotSuccessful() => Result.Success.Should().BeFalse();

            public async Task InitializeAsync() => Result = await Manager.Create(Client);

            public async Task DisposeAsync() => await Task.CompletedTask;
        }
    }
}