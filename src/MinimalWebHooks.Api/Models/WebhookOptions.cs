using Microsoft.AspNetCore.Authorization;

namespace MinimalWebHooks.Api.Models;

public class WebhookApiOptions
{
    public AuthorizationPolicy? AuthPolicy;
    public WebhookEventsWorkerOptions? WorkerOptions;
}