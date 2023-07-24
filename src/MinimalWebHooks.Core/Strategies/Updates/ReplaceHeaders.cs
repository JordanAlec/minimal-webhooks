using MinimalWebHooks.Core.Interfaces;
using MinimalWebHooks.Core.Models;
using MinimalWebHooks.Core.Models.DbSets;

namespace MinimalWebHooks.Core.Strategies.Updates;

public class ReplaceHeaders : IUpdateStrategy
{
    private readonly WebhookUpdateCommand _command;
    private readonly WebhookClient _client;
    private readonly IWebhookDataStore _dataStore;

    public ReplaceHeaders(WebhookUpdateCommand command, WebhookClient client, IWebhookDataStore dataStore)
    {
        _command = command;
        _client = client;
        _dataStore = dataStore;
    }

    public bool ShouldUpdate() => _command.HasHeaderReplacements();

    public async Task<UpdateResult> Update()
    {
        if (_client.HasHeaders()) await _dataStore.Delete(_client.ClientHeaders!);
        _client.ClientHeaders = _command.ReplaceHeaders;

        return new UpdateResult().ResultSuccess();
    }
}