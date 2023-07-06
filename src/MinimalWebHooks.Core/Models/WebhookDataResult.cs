namespace MinimalWebHooks.Core.Models;

public class WebhookDataResult
{
    public bool Success { get; private set; }
    public string Message { get; private set; }
    public List<WebhookClient> Data { get; }

    public WebhookDataResult() => Data = new List<WebhookClient>();

    private WebhookDataResult Result(bool success, string message, params WebhookClient[]? clients)
    {
        Success = success;
        Message = message;
        if (clients != null && clients.Any()) Data.AddRange(clients);
        return this;
    }

    public WebhookDataResult FailedResult(string message, params WebhookClient[]? clients) => 
        Result(false, message, clients);

    public WebhookDataResult SuccessfulResult(string message, params WebhookClient[]? clients) => 
        Result(true, message, clients);
}