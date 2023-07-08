using Microsoft.EntityFrameworkCore;
using MinimalWebHooks.Api.Builders;
using MinimalWebHooks.Core.Builders;
using MinimalWebHooks.Core.Extensions;

namespace MinimalWebHooks.Api.Extensions.Startup;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddMinimalWebhooksApi(this IServiceCollection services, Action<DbContextOptionsBuilder> dbContextOptions, Action<WebhookApiOptionsBuilder> webhookApiOptions, Action<WebhookOptionsBuilder> webhookOptions)
    {
        services.AddAuthorization();
        services.AddEndpointsApiExplorer();

        var webhookApiOptionsBuilder = new WebhookApiOptionsBuilder();
        webhookApiOptions(webhookApiOptionsBuilder);
        var builtOptions = webhookApiOptionsBuilder.Build();

        services.AddAuthorization(options =>
        {
            options.AddPolicy("WebhookClientPolicy", builtOptions.AuthPolicy);
        });

        services.AddMinimalWebhooksCore(dbContextOptions, webhookOptions);
        return services;
    }
}