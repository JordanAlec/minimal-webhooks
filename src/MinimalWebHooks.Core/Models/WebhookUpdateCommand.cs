namespace MinimalWebHooks.Core.Models;

public class WebhookUpdateCommand
{
    public int Id { get; set; }
    public bool SetDisabledFlag { get; set; }
    public Dictionary<string, string>? ReplaceHeaders { get; set; }
    public bool HasHeaderReplacements() => ReplaceHeaders != null && ReplaceHeaders.Any();
}