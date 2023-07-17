using Microsoft.AspNetCore.Authorization;
using MinimalWebHooks.Api.Models;
using MinimalWebHooks.Core.Interfaces;
using MinimalWebHooks.Core.Models;

namespace MinimalWebHooks.Api.Builders;

public class WebhookApiOptionsBuilder
{
    private readonly WebhookApiOptions _options;

    public WebhookApiOptionsBuilder() => _options = new WebhookApiOptions();

   
    /// <summary>
    /// Setting this allows you to define the authorisation policy for endpoints GET / CREATE / DELETE webhook clients
    /// </summary>
    /// <param name="authPolicy"></param>
    /// <returns></returns>
    public WebhookApiOptionsBuilder SetAuthorizationPolicy(AuthorizationPolicy authPolicy)
    {
        _options.AuthPolicy = authPolicy;
        return this;
    }

    /// <summary>
    /// Setting this will enable a worker that will periodically check for events and send them. If omitted no worker will be registered and events must be sent via the 'WebhookEventsManager'
    /// You can pass in the timer value in milliseconds by calling 'SetWorkerOptions' instead.
    /// This will set a default timer value of ten minutes.
    /// </summary>
    /// <returns></returns>
    public WebhookApiOptionsBuilder EnableWorker()
    {
        _options.WorkerOptions = new WebhookEventsWorkerOptions
        {
            EnableWorker = true,
            TimerValue = 600000
        };
        return this;
    }

    /// <summary>
    /// Setting this will enable a worker that will periodically check for events and send them. If omitted no worker will be registered and events must be sent via the 'WebhookEventsManager'
    /// </summary>
    /// <param name="workerTimerValue"></param>
    /// <returns></returns>
    public WebhookApiOptionsBuilder SetWorkerOptions(double workerTimerValue)
    {
        _options.WorkerOptions = new WebhookEventsWorkerOptions
        {
            EnableWorker = true,
            TimerValue = workerTimerValue
        };
        return this;
    }

    internal WebhookApiOptions Build() => _options;
}