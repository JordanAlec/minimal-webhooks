using MinimalWebHooks.Core.Models.DbSets;

namespace MinimalWebHooks.Core.Models;

public class WebhookUpdateCommand
{
    public int Id { get; set; }
    public bool? SetDisabledFlag { get; set; }
    public string? WebhookUrl { get; set; }
    public bool? DeleteAllHeaders { get; set; }
    public List<WebhookClientHeader>? ReplaceHeaders { get; set; }
    public bool HasDisabledFlag() => SetDisabledFlag.HasValue;
    public bool HasWebhookUrl() => !string.IsNullOrWhiteSpace(WebhookUrl);
    public bool HasDeleteHeadersFlag() => DeleteAllHeaders.HasValue && DeleteAllHeaders.Value;
    public bool HasHeaderReplacements() => ReplaceHeaders != null && ReplaceHeaders.Any();
}