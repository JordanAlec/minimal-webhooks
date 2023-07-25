using MinimalWebHooks.Core.Enum;
using MinimalWebHooks.Core.Models.DbSets;

namespace MinimalWebHooks.Tests;

public class WebhookEventsManagerSpec
{
    public class CanSendEvents : WebhookEventsManagerBaseSpec, IAsyncLifetime
    {
        private static readonly WebhookClient WebhookClient = FakeData.WebhookClient(WebhookActionType.Create);
        private static readonly WebhookActionEvent WebhookActionEvent = FakeData.WebhookActionEvent(WebhookClient, WebhookActionType.Create);

        public CanSendEvents() : base(
            new MockWebhookDataStoreBuilder().SetupClients(new List<WebhookClient>{ WebhookClient }).SetupClientsGetByEntity(new List<WebhookClient> { WebhookClient }),
            new MockWebhookActionEventProcessorBuilder().Setup(new List<WebhookActionEvent>{ WebhookActionEvent }, true), 
            new MockWebhookClientHttpClientBuilder().Setup(WebhookActionEvent, WebhookClient, true))
        {}

        [Fact]
        public void EventsProcessorGetsEvents()
        {
            EventsProcessor.Verify(x => x.HasEvents(), Times.Once);
            EventsProcessor.Verify(x => x.GetEvents(), Times.Once);
            EventsProcessor.VerifyNoOtherCalls();
        }

        [Fact]
        public void DataStoreCanGetClientByEntity()
        {
            DataStore.Verify(x => x.GetByEntity<object>(WebhookClient, WebhookActionType.Create));
            DataStore.Verify(x => x.Update(WebhookClient));
            DataStore.VerifyNoOtherCalls();
        }

        [Fact]
        public void HttpClientCallsWebhookUrl()
        {
            WebhookHttpClient.Verify(x => x.SendEventToWebhookUrl(WebhookActionEvent, WebhookClient));
            WebhookHttpClient.VerifyNoOtherCalls();
        }

        [Fact]
        public void EventResultsAllSuccessful() => EventResults.All(x => x.Success).Should().BeTrue();

        public async Task InitializeAsync() => EventResults = await Manager.SendEvents();

        public async Task DisposeAsync() => await Task.CompletedTask;
    }

    public class CanSendEventsFailureResponse : WebhookEventsManagerBaseSpec, IAsyncLifetime
    {
        private static readonly WebhookClient WebhookClient = FakeData.WebhookClient(WebhookActionType.Create);
        private static readonly WebhookActionEvent WebhookActionEvent = FakeData.WebhookActionEvent(WebhookClient, WebhookActionType.Create);

        public CanSendEventsFailureResponse() : base(
            new MockWebhookDataStoreBuilder().SetupClients(new List<WebhookClient> { WebhookClient }).SetupClientsGetByEntity(new List<WebhookClient> { WebhookClient }),
            new MockWebhookActionEventProcessorBuilder().Setup(new List<WebhookActionEvent> { WebhookActionEvent }, true),
            new MockWebhookClientHttpClientBuilder().Setup(WebhookActionEvent, WebhookClient, false))
        { }

        [Fact]
        public void EventsProcessorGetsEvents()
        {
            EventsProcessor.Verify(x => x.HasEvents(), Times.Once);
            EventsProcessor.Verify(x => x.GetEvents(), Times.Once);
            EventsProcessor.VerifyNoOtherCalls();
        }

        [Fact]
        public void DataStoreCanGetClientByEntity()
        {
            DataStore.Verify(x => x.GetByEntity<object>(WebhookClient, WebhookActionType.Create));
            DataStore.Verify(x => x.Update(WebhookClient));
            DataStore.VerifyNoOtherCalls();
        }

        [Fact]
        public void HttpClientCallsWebhookUrl()
        {
            WebhookHttpClient.Verify(x => x.SendEventToWebhookUrl(WebhookActionEvent, WebhookClient));
            WebhookHttpClient.VerifyNoOtherCalls();
        }

        [Fact]
        public void EventResultsAllSuccessful() => EventResults.All(x => x.Success).Should().BeFalse();

        public async Task InitializeAsync() => EventResults = await Manager.SendEvents();

        public async Task DisposeAsync() => await Task.CompletedTask;
    }

    public class CannotSendEvents : WebhookEventsManagerBaseSpec, IAsyncLifetime
    {
        private static readonly WebhookClient WebhookClient = FakeData.WebhookClient(WebhookActionType.Update);
        private static readonly WebhookActionEvent WebhookActionEvent = FakeData.WebhookActionEvent(WebhookClient, WebhookActionType.Update);

        public CannotSendEvents() : base(
            new MockWebhookDataStoreBuilder().SetupClients(new List<WebhookClient> { WebhookClient }),
            new MockWebhookActionEventProcessorBuilder().Setup(FakeData.WebhookActionEvents(WebhookClient, 1, WebhookActionType.Update), true),
            new MockWebhookClientHttpClientBuilder().Setup(WebhookActionEvent, WebhookClient, true))
        { }

        [Fact]
        public void EventsProcessorGetsEvents()
        {
            EventsProcessor.Verify(x => x.HasEvents(), Times.Once);
            EventsProcessor.Verify(x => x.GetEvents(), Times.Once);
            EventsProcessor.VerifyNoOtherCalls();
        }

        [Fact]
        public void DataStoreCanGetClientByEntity()
        {
            DataStore.Verify(x => x.GetByEntity<object>(WebhookClient, WebhookActionType.Update));
            DataStore.VerifyNoOtherCalls();
        }

        [Fact]
        public void EventResultsAllEmpty() => EventResults.Should().BeEmpty();

        [Fact]
        public void HttpClientDoesntNotMakeAnyCalls() => WebhookHttpClient.VerifyNoOtherCalls();

        public async Task InitializeAsync() => EventResults = await Manager.SendEvents();

        public async Task DisposeAsync() => await Task.CompletedTask;
    }

    public class CanWriteEvents : WebhookEventsManagerBaseSpec, IAsyncLifetime
    {
        private static readonly WebhookActionEvent WebhookActionEvent =
            FakeData.WebhookActionEvent(FakeData.WebhookClient(WebhookActionType.Create), WebhookActionType.Create);

        public CanWriteEvents() : base(
            new MockWebhookDataStoreBuilder(),
            new MockWebhookActionEventProcessorBuilder().Setup(new List<WebhookActionEvent>{ WebhookActionEvent }, true),
            new MockWebhookClientHttpClientBuilder())
        { }

        [Fact]
        public void EventsProcessorWritesEvents()
        {
            EventsProcessor.Verify(x => x.WriteEvent(WebhookActionEvent), Times.Once);
            EventsProcessor.VerifyNoOtherCalls();
        }

        public async Task InitializeAsync() => await Manager.WriteEvent(WebhookActionEvent);

        public async Task DisposeAsync() => await Task.CompletedTask;
    }
}