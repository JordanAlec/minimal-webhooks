using MinimalWebHooks.Core.Enum;

namespace MinimalWebHooks.Core.Models;

public class WebhookClient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string WebhookUrl { get; set; }
    public WebhookActionType ActionType { get; set; }
    public string EntityTypeName { get; set; }
}