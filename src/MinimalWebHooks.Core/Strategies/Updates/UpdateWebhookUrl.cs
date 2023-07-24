using MinimalWebHooks.Core.Interfaces;
using MinimalWebHooks.Core.Models;
using MinimalWebHooks.Core.Models.DbSets;
using MinimalWebHooks.Core.Validation.Rules;

namespace MinimalWebHooks.Core.Strategies.Updates;

public class UpdateWebhookUrl : IUpdateStrategy
{
    private readonly WebhookUpdateCommand _command;
    private readonly WebhookClient _client;
    private readonly WebhookClientUrlCanBeReached _urlCanBeReached;

    public UpdateWebhookUrl(WebhookUpdateCommand command, WebhookClient client, IWebhookClientHttpClient httpClient)
    {
        _command = command;
        _client = client;
        _urlCanBeReached = new WebhookClientUrlCanBeReached(client, httpClient);
    }

    public bool ShouldUpdate() => _command.HasWebhookUrl();

    public async Task<UpdateResult> Update()
    {
        _client.WebhookUrl = _command.WebhookUrl!;
        var validationResult = await _urlCanBeReached.Validate();
        return !validationResult.IsValid ? new UpdateResult().ResultFailure(validationResult.Message) : new UpdateResult().ResultSuccess();
    }
}