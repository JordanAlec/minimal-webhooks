using System.ComponentModel.DataAnnotations;

namespace MinimalWebHooks.Core.Models.DbSets;

public class WebhookClientHeader
{
    public int Id { get; set; }
    [Required]
    public string Key { get; set; }
    [Required]
    public string Value { get; set; }
}