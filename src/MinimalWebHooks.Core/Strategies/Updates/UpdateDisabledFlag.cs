using MinimalWebHooks.Core.Interfaces;
using MinimalWebHooks.Core.Models;
using MinimalWebHooks.Core.Models.DbSets;

namespace MinimalWebHooks.Core.Strategies.Updates;

public class UpdateDisabledFlag : IUpdateStrategy
{
    private readonly WebhookUpdateCommand _command;
    private readonly WebhookClient _client;

    public UpdateDisabledFlag(WebhookUpdateCommand command, WebhookClient client)
    {
        _command = command;
        _client = client;
    }
    public bool ShouldUpdate() => _command.HasDisabledFlag();

    public async Task<UpdateResult> Update()
    {
        _client.Disabled = _command.SetDisabledFlag!.Value;
        return new UpdateResult().ResultSuccess();
    }
}