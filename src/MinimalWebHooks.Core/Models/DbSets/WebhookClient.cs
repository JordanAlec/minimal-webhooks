using System.ComponentModel.DataAnnotations;
using MinimalWebHooks.Core.Enum;

namespace MinimalWebHooks.Core.Models.DbSets;

public class WebhookClient
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string WebhookUrl { get; set; }
    [Required]
    public WebhookActionType ActionType { get; set; }
    [Required]
    public string EntityTypeName { get; set; }
    public bool Disabled { get; set; }
    public List<WebhookClientHeader>? ClientHeaders { get; set; }

    public bool HasHeaders() => ClientHeaders != null && ClientHeaders.Any();
}