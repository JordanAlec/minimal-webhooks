using Microsoft.Extensions.Logging;
using MinimalWebHooks.Core.Enum;
using MinimalWebHooks.Core.Models.DbSets;

namespace MinimalWebHooks.Tests;

public class WebhookEventsManagerSpec
{
    public class CanSendEvents : IAsyncLifetime
    {
        private readonly Mock<IWebhookDataStore> _dataStore;
        private readonly Mock<IWebhookActionEventProcessor> _eventsProcessor;
        private readonly Mock<IWebhookClientHttpClient> _webhookHttpClient;
        private readonly WebhookEventsManager _manager;
        private List<WebhookActionEventResult> _eventResults;

        private readonly WebhookClient _webhookClient;
        private readonly WebhookActionEvent _webhookActionEvent;

        public CanSendEvents()
        {
            _webhookClient = FakeData.WebhookClient(WebhookActionType.Create);
            _webhookActionEvent = FakeData.WebhookActionEvent(_webhookClient, WebhookActionType.Create);

            _dataStore = new MockWebhookDataStoreBuilder().SetupClients(new List<WebhookClient> { _webhookClient }).SetupClientsGetByEntity(new List<WebhookClient> { _webhookClient }).Build();
            _webhookHttpClient = new MockWebhookClientHttpClientBuilder().Setup(_webhookActionEvent, _webhookClient, true).Build();
            _eventsProcessor = new MockWebhookActionEventProcessorBuilder().Setup(new List<WebhookActionEvent> { _webhookActionEvent }, true).Build();
            _manager = new WebhookEventsManager(new Mock<ILogger<WebhookEventsManager>>().Object, _dataStore.Object, _eventsProcessor.Object, _webhookHttpClient.Object);
        }

        [Fact]
        public void EventsProcessorGetsEvents()
        {
            _eventsProcessor.Verify(x => x.HasEvents(), Times.Once);
            _eventsProcessor.Verify(x => x.GetEvents(), Times.Once);
            _eventsProcessor.VerifyNoOtherCalls();
        }

        [Fact]
        public void DataStoreCanGetClientByEntity()
        {
            _dataStore.Verify(x => x.GetByEntity<object>(_webhookClient, WebhookActionType.Create));
            _dataStore.Verify(x => x.Update(_webhookClient));
            _dataStore.VerifyNoOtherCalls();
        }

        [Fact]
        public void HttpClientCallsWebhookUrl()
        {
            _webhookHttpClient.Verify(x => x.SendEventToWebhookUrl(_webhookActionEvent, _webhookClient));
            _webhookHttpClient.VerifyNoOtherCalls();
        }

        [Fact]
        public void EventResultsAllSuccessful() => _eventResults.All(x => x.Success).Should().BeTrue();

        public async Task InitializeAsync() => _eventResults = await _manager.SendEvents();

        public async Task DisposeAsync() => await Task.CompletedTask;
    }

    public class CanSendEventsFailureResponse : IAsyncLifetime
    {
        private readonly Mock<IWebhookDataStore> _dataStore;
        private readonly Mock<IWebhookActionEventProcessor> _eventsProcessor;
        private readonly Mock<IWebhookClientHttpClient> _webhookHttpClient;
        private readonly WebhookEventsManager _manager;
        private List<WebhookActionEventResult> _eventResults;

        private readonly WebhookClient _webhookClient;
        private readonly WebhookActionEvent _webhookActionEvent;

        public CanSendEventsFailureResponse()
        {
            _webhookClient = FakeData.WebhookClient(WebhookActionType.Create);
            _webhookActionEvent = FakeData.WebhookActionEvent(_webhookClient, WebhookActionType.Create);

            _dataStore = new MockWebhookDataStoreBuilder().SetupClients(new List<WebhookClient> { _webhookClient }).SetupClientsGetByEntity(new List<WebhookClient> { _webhookClient }).Build();
            _webhookHttpClient = new MockWebhookClientHttpClientBuilder().Setup(_webhookActionEvent, _webhookClient, false).Build();
            _eventsProcessor = new MockWebhookActionEventProcessorBuilder().Setup(new List<WebhookActionEvent> { _webhookActionEvent }, true).Build();
            _manager = new WebhookEventsManager(new Mock<ILogger<WebhookEventsManager>>().Object, _dataStore.Object, _eventsProcessor.Object, _webhookHttpClient.Object);
        }

        [Fact]
        public void EventsProcessorGetsEvents()
        {
            _eventsProcessor.Verify(x => x.HasEvents(), Times.Once);
            _eventsProcessor.Verify(x => x.GetEvents(), Times.Once);
            _eventsProcessor.VerifyNoOtherCalls();
        }

        [Fact]
        public void DataStoreCanGetClientByEntity()
        {
            _dataStore.Verify(x => x.GetByEntity<object>(_webhookClient, WebhookActionType.Create));
            _dataStore.Verify(x => x.Update(_webhookClient));
            _dataStore.VerifyNoOtherCalls();
        }

        [Fact]
        public void HttpClientCallsWebhookUrl()
        {
            _webhookHttpClient.Verify(x => x.SendEventToWebhookUrl(_webhookActionEvent, _webhookClient));
            _webhookHttpClient.VerifyNoOtherCalls();
        }

        [Fact]
        public void EventResultsAllSuccessful() => _eventResults.All(x => x.Success).Should().BeFalse();

        public async Task InitializeAsync() => _eventResults = await _manager.SendEvents();

        public async Task DisposeAsync() => await Task.CompletedTask;
    }

    public class CannotSendEvents : IAsyncLifetime
    {
        private readonly Mock<IWebhookDataStore> _dataStore;
        private readonly Mock<IWebhookActionEventProcessor> _eventsProcessor;
        private readonly Mock<IWebhookClientHttpClient> _webhookHttpClient;
        private readonly WebhookEventsManager _manager;
        private List<WebhookActionEventResult> _eventResults;

        private readonly WebhookClient _webhookClient;
        private readonly WebhookActionEvent _webhookActionEvent;

        public CannotSendEvents()
        {
            _webhookClient = FakeData.WebhookClient(WebhookActionType.Update);
            _webhookActionEvent = FakeData.WebhookActionEvent(_webhookClient, WebhookActionType.Update);

            _dataStore = new MockWebhookDataStoreBuilder().SetupClients(new List<WebhookClient> { _webhookClient }).Build();
            _webhookHttpClient = new MockWebhookClientHttpClientBuilder().Setup(_webhookActionEvent, _webhookClient, true).Build();
            _eventsProcessor = new MockWebhookActionEventProcessorBuilder().Setup(FakeData.WebhookActionEvents(_webhookClient, 1, WebhookActionType.Update), true).Build();
            _manager = new WebhookEventsManager(new Mock<ILogger<WebhookEventsManager>>().Object, _dataStore.Object, _eventsProcessor.Object, _webhookHttpClient.Object);
        }

        [Fact]
        public void EventsProcessorGetsEvents()
        {
            _eventsProcessor.Verify(x => x.HasEvents(), Times.Once);
            _eventsProcessor.Verify(x => x.GetEvents(), Times.Once);
            _eventsProcessor.VerifyNoOtherCalls();
        }

        [Fact]
        public void DataStoreCanGetClientByEntity()
        {
            _dataStore.Verify(x => x.GetByEntity<object>(_webhookClient, WebhookActionType.Update));
            _dataStore.VerifyNoOtherCalls();
        }

        [Fact]
        public void EventResultsAllEmpty() => _eventResults.Should().BeEmpty();

        [Fact]
        public void HttpClientDoesntNotMakeAnyCalls() => _webhookHttpClient.VerifyNoOtherCalls();

        public async Task InitializeAsync() => _eventResults = await _manager.SendEvents();

        public async Task DisposeAsync() => await Task.CompletedTask;
    }

    public class CanWriteEvents : IAsyncLifetime
    {
        private readonly Mock<IWebhookDataStore> _dataStore;
        private readonly Mock<IWebhookActionEventProcessor> _eventsProcessor;
        private readonly MockWebhookActionEventProcessorBuilder _eventsProcessorBuilder;
        private readonly WebhookEventsManager _manager;

        private readonly WebhookActionEvent _webhookActionEvent;

        public CanWriteEvents()
        {
            _webhookActionEvent = FakeData.WebhookActionEvent(FakeData.WebhookClient(WebhookActionType.Create), WebhookActionType.Create);
            _dataStore = new MockWebhookDataStoreBuilder().Build();
            var webhookHttpClient = new MockWebhookClientHttpClientBuilder().Build();
            _eventsProcessorBuilder = new MockWebhookActionEventProcessorBuilder().Setup(new List<WebhookActionEvent> { _webhookActionEvent }, true);
            _eventsProcessor = _eventsProcessorBuilder.Build();
            _manager = new WebhookEventsManager(new Mock<ILogger<WebhookEventsManager>>().Object, _dataStore.Object, _eventsProcessor.Object, webhookHttpClient.Object);
        }

        [Fact]
        public void EventsProcessorWritesEvents()
        {
            _eventsProcessor.Verify(x => x.WriteEvent(_webhookActionEvent), Times.Once);
            _eventsProcessor.VerifyNoOtherCalls();
        }

        [Fact]
        public void EventWritten() => _eventsProcessorBuilder.GetWrittenEvents().Should().ContainEquivalentOf(_webhookActionEvent);

        public async Task InitializeAsync() => await _manager.WriteEvent(_webhookActionEvent);

        public async Task DisposeAsync() => await Task.CompletedTask;
    }

    public class CanWriteEventsWithUdfs : IAsyncLifetime
    {
        private readonly Mock<IWebhookDataStore> _dataStore;
        private readonly Mock<IWebhookActionEventProcessor> _eventsProcessor;
        private readonly MockWebhookActionEventProcessorBuilder _eventsProcessorBuilder;
        private readonly WebhookEventsManager _manager;

        private readonly WebhookActionEvent _webhookActionEvent;

        public CanWriteEventsWithUdfs()
        {
            _webhookActionEvent = FakeData.WebhookActionEvent(FakeData.WebhookClient(WebhookActionType.Create), WebhookActionType.Create);
            _webhookActionEvent.AddUdf(FakeData.WebhookActionEventUdf());
            _dataStore = new MockWebhookDataStoreBuilder().Build();
            var webhookHttpClient = new MockWebhookClientHttpClientBuilder().Build();
            _eventsProcessorBuilder = new MockWebhookActionEventProcessorBuilder().Setup(new List<WebhookActionEvent> {_webhookActionEvent}, true);
            _eventsProcessor = _eventsProcessorBuilder.Build();
            _manager = new WebhookEventsManager(new Mock<ILogger<WebhookEventsManager>>().Object, _dataStore.Object, _eventsProcessor.Object, webhookHttpClient.Object);
        }

        [Fact]
        public void EventsProcessorWritesEvents()
        {
            _eventsProcessor.Verify(x => x.WriteEvent(_webhookActionEvent), Times.Once);
            _eventsProcessor.VerifyNoOtherCalls();
        }

        [Fact]
        public void EventWritten() => _eventsProcessorBuilder.GetWrittenEvents().Should().ContainEquivalentOf(_webhookActionEvent);

        public async Task InitializeAsync() => await _manager.WriteEvent(_webhookActionEvent);

        public async Task DisposeAsync() => await Task.CompletedTask;
    }
}