using MinimalWebHooks.Core.Models.DbSets;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MinimalWebHooks.Core.Models;

public class WebhookActionEventResult
{
    public bool Success { get; private set; }
    public string? Message { get; private set; }
    public DateTime SentDate { get; private set; }
    public WebhookActionEvent WebhookActionEvent { get; private set; }
    public WebhookClient WebhookClient { get; private set; }

    private WebhookActionEventResult Result(bool success, WebhookActionEvent actionEvent, WebhookClient client, string? message = null)
    {
        Success = success;
        Message = message;
        SentDate = DateTime.Now;
        WebhookActionEvent = actionEvent;
        WebhookClient = client;
        return this;
    }

    public WebhookActionEventResult FailedResult(WebhookActionEvent actionEvent, WebhookClient client, string? message = null) =>
        Result(false, actionEvent, client, message);

    public WebhookActionEventResult SuccessfulResult(WebhookActionEvent actionEvent, WebhookClient client, string? message = null) =>
        Result(true, actionEvent, client, message);
}