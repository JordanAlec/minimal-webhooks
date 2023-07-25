using MinimalWebHooks.Core.Models.DbSets;

namespace MinimalWebHooks.Core.Models;

public class WebhookActionEventResult
{
    public bool Success { get; private set; }
    public int StatusCode { get; private set; }
    public string? Message { get; private set; }
    public DateTime SentDate { get; private set; }
    public WebhookActionEvent WebhookActionEvent { get; private set; }
    public WebhookClient WebhookClient { get; private set; }

    private WebhookActionEventResult Result(bool success, int statusCode, WebhookActionEvent actionEvent, WebhookClient client, string? message = null)
    {
        Success = success;
        StatusCode = statusCode;
        Message = message;
        SentDate = DateTime.Now;
        WebhookActionEvent = actionEvent;
        WebhookClient = client;
        return this;
    }

    public WebhookActionEventResult FailedResult(WebhookActionEvent actionEvent, WebhookClient client, int statusCode, string? message = null) =>
        Result(false, statusCode, actionEvent, client, message);

    public WebhookActionEventResult SuccessfulResult(WebhookActionEvent actionEvent, WebhookClient client, int statusCode, string? message = null) =>
        Result(true, statusCode, actionEvent, client, message);
}