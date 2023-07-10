using MinimalWebHooks.Core.Models.DbSets;

namespace MinimalWebHooks.Core.Models;

public class WebhookUpdateCommand
{
    public int Id { get; set; }
    public bool SetDisabledFlag { get; set; }
    public List<WebhookClientHeader>? ReplaceHeaders { get; set; }
    public bool HasHeaderReplacements() => ReplaceHeaders != null && ReplaceHeaders.Any();
}