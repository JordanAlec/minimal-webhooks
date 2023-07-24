using MinimalWebHooks.Core.Interfaces;
using MinimalWebHooks.Core.Models;
using MinimalWebHooks.Core.Models.DbSets;

namespace MinimalWebHooks.Core.Strategies.Updates;

public class RemoveAllHeaders : IUpdateStrategy
{
    private readonly WebhookUpdateCommand _command;
    private readonly WebhookClient _client;
    private readonly IWebhookDataStore _dataStore;

    public RemoveAllHeaders(WebhookUpdateCommand command, WebhookClient client, IWebhookDataStore dataStore)
    {
        _command = command;
        _client = client;
        _dataStore = dataStore;
    }

    public bool ShouldUpdate() => _command.HasDeleteHeadersFlag() && _client.HasHeaders();

    public async Task<UpdateResult> Update()
    {
        await _dataStore.Delete(_client.ClientHeaders!);
        return new UpdateResult().ResultSuccess();
    }
}