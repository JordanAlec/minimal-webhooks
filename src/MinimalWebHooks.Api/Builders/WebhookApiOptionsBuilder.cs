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

    internal WebhookApiOptions Build() => _options;
}