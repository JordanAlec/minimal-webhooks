using MinimalWebHooks.Core.Models;

namespace MinimalWebHooks.Core.Validation;

public class WebhookValidationResult
{
    public List<ValidationResult>? ValidationResults { get; private set; }
    public WebhookClient? ValidatedClient { get; private set; }

    public bool HasValidationResults() => ValidationResults != null && ValidationResults.Any();
    public bool IsValid() => HasValidationResults() && ValidationResults.All(x => x.IsValid);
    public string GetMessage() => HasValidationResults() ? string.Join(" ", ValidationResults.Where(x => !string.IsNullOrWhiteSpace(x.Message)).Select(x => x.Message)) : string.Empty;

    public WebhookValidationResult AddResult(ValidationResult result)
    {
        ValidationResults ??= new List<ValidationResult>();
        ValidationResults.Add(result);
        return this;
    }

    public WebhookValidationResult AddClient(WebhookClient client)
    {
        ValidatedClient = client;
        return this;
    }
}