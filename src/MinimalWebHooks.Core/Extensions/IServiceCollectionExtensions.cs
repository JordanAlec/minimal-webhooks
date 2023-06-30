using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MinimalWebHooks.Core.Builders;
using MinimalWebHooks.Core.DataStore;
using MinimalWebHooks.Core.Interfaces;
using MinimalWebHooks.Core.Managers;
using MinimalWebHooks.Core.Processors;

namespace MinimalWebHooks.Core.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddMinimalWebhooksCore(this IServiceCollection services, Action<DbContextOptionsBuilder> dbContextOptions, Action<MinimalWebhookOptionsBuilder> webhookOptions)
    {
        var webhookOptionsBuilder = new MinimalWebhookOptionsBuilder();
        webhookOptions(webhookOptionsBuilder);

        services.AddSingleton(webhookOptionsBuilder.Build());
        services.AddDbContext<MinimalWebhooksDbContext>(dbContextOptions);
        services.AddTransient<IWebhookDataStore, WebhookDataStore>();
        services.AddTransient<IMinimalWebhookOptionsProcessor, MinimalWebhookOptionsProcessor>();
        services.AddTransient<WebhookClientManager>();
        return services;
    }
}