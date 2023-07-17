using Microsoft.AspNetCore.Authorization;

namespace MinimalWebHooks.Web.Models;

public class WebhookApiOptions
{
    public AuthorizationPolicy? AuthPolicy;
    public WebhookEventsWorkerOptions? WorkerOptions;
}