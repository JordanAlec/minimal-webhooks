using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MinimalWebHooks.Core.Builders;
using MinimalWebHooks.Core.Extensions;
using MinimalWebHooks.Web.Builders;
using MinimalWebHooks.Web.Models;

namespace MinimalWebHooks.Web.Extensions.Startup;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddMinimalWebhooksApi(this IServiceCollection services, Action<DbContextOptionsBuilder> dbContextOptions, Action<WebhookApiOptionsBuilder> webhookApiOptions, Action<WebhookOptionsBuilder> webhookOptions)
    {
        var webhookApiOptionsBuilder = new WebhookApiOptionsBuilder();
        webhookApiOptions(webhookApiOptionsBuilder);
        var builtOptions = webhookApiOptionsBuilder.Build();

        services.AddAuthorization(options =>
        {
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