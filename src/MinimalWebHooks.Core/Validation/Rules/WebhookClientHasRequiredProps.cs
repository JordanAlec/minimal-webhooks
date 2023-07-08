using MinimalWebHooks.Core.Interfaces;
using MinimalWebHooks.Core.Models;

namespace MinimalWebHooks.Core.Validation.Rules;

public class WebhookClientHasRequiredProps : IValidationRule
{
    private readonly WebhookClient _client;

    public WebhookClientHasRequiredProps(WebhookClient client) => _client = client;

    public async Task<ValidationResult> Validate()
    {
        if (string.IsNullOrWhiteSpace(_client.Name)) return new ValidationResult().Failure("Client must have 'Name'.");
        if (string.IsNullOrWhiteSpace(_client.WebhookUrl)) return new ValidationResult().Failure("Client must have 'WebhookUrl'.");
        if (string.IsNullOrWhiteSpace(_client.EntityTypeName)) return new ValidationResult().Failure("Client must have 'EntityTypeName'.");

        return new ValidationResult().Success($"Passed validation: {nameof(WebhookClientHasRequiredProps)}.");
    }
}