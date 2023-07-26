namespace MinimalWebHooks.Core.Models;

public class WebhookActionEventUdf
{
    public string Key { get; private set; }
    public string Value { get; private set; }

    public WebhookActionEventUdf Create(string key, string value)
    {
        Key = key;
        Value = value;
        return this;
    }
}