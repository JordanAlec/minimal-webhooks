using MinimalWebHooks.Core.Interfaces;
using MinimalWebHooks.Core.Models;

namespace MinimalWebHooks.Core.Validation;

public class WebhookClientValidator
{
    private readonly WebhookValidationResult _result;
    private readonly List<IValidationRule> _rules;

    public WebhookClientValidator(WebhookClient client, List<IValidationRule> rules)
    {
        _result = new WebhookValidationResult().AddClient(client);
        _rules = rules;
    }

    public async Task<WebhookValidationResult> ValidateClient()
    {
        foreach (var rule in _rules) _result.AddResult(await rule.Validate());
        return _result;
    }
}