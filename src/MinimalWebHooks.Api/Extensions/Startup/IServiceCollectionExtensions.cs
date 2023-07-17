using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MinimalWebHooks.Api.Builders;
using MinimalWebHooks.Api.Models;
using MinimalWebHooks.Core.Builders;
using MinimalWebHooks.Core.Extensions;

namespace MinimalWebHooks.Api.Extensions.Startup;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddMinimalWebhooksApi(this IServiceCollection services, Action<DbContextOptionsBuilder> dbContextOptions, Action<WebhookApiOptionsBuilder> webhookApiOptions, Action<WebhookOptionsBuilder> webhookOptions)
    {
        var webhookApiOptionsBuilder = new WebhookApiOptionsBuilder();
        webhookApiOptions(webhookApiOptionsBuilder);
        var builtOptions = webhookApiOptionsBuilder.Build();

        services.AddAuthorization(options =>
        {
            var a = services.Any(x => x.ServiceType == typeof(IAuthenticationService));
            var b = services.Any(x => x.ServiceType == typeof(IAuthenticationSchemeProvider));
            var policy = builtOptions.AuthPolicy ?? new AuthorizationPolicyBuilder().RequireAssertion(context => true).Build();
            options.AddPolicy(ApiConstants.Policy.WebhookPolicyName, policy);
        });

        services.AddEndpointsApiExplorer();
        services.AddMinimalWebhooksCore(dbContextOptions, webhookOptions);
        return services.AddMinimalWebhooksWorker(builtOptions);
    }

    private static IServiceCollection AddMinimalWebhooksWorker(this IServiceCollection services, WebhookApiOptions options)
    {
        if (options.WorkerOptions is not {EnableWorker: true}) return services;

        services.AddSingleton(options.WorkerOptions);
        services.AddHostedService<SendEventsWorker>();

        return services;
    }
}