using Microsoft.EntityFrameworkCore;
using MinimalWebHooks.Core.Builders;
using MinimalWebHooks.Core.Extensions;

namespace MinimalWebHooks.Api.Extensions.Startup;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddMinimalWebhooksApi(this IServiceCollection services, Action<DbContextOptionsBuilder> dbContextOptions, Action<WebhookOptionsBuilder> webhookOptions)
    {
        services.AddAuthorization();
        services.AddEndpointsApiExplorer();
        services.AddMinimalWebhooksCore(dbContextOptions, webhookOptions);
        return services;
    }
}