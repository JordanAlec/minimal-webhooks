﻿using MinimalWebHooks.Core.Interfaces;
using MinimalWebHooks.Core.Models.DbSets;

namespace MinimalWebHooks.Core.Validation.Rules;

public class WebhookClientUrlCanBeReached : IValidationRule
{
    private readonly WebhookClient _client;
    private readonly IWebhookClientHttpClient _httpClient;

    public WebhookClientUrlCanBeReached(WebhookClient client, IWebhookClientHttpClient httpClient)
    {
        _client = client;
        _httpClient = httpClient;
    }

    public async Task<ValidationResult> Validate()
    {
        var canVerifyWebhookUrl = await _httpClient.VerifyWebhookUrl(_client);
        return canVerifyWebhookUrl ? 
            new ValidationResult().Success("Client 'WebhookUrl' has been verified with a HEAD request or 'WebhookUrlIsReachable' has not been set.") : 
            new ValidationResult().Failure("Cannot verify client 'WebhookUrl'. Make sure the URL can receive a HEAD request or do not set 'WebhookUrlIsReachable'.");
    }
}