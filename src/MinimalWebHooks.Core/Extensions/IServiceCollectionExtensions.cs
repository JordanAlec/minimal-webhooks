using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MinimalWebHooks.Core.Builders;
using MinimalWebHooks.Core.DataStore;
using MinimalWebHooks.Core.Http;
using MinimalWebHooks.Core.Interfaces;
using MinimalWebHooks.Core.Managers;
using MinimalWebHooks.Core.Processors;

namespace MinimalWebHooks.Core.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddMinimalWebhooksCore(this IServiceCollection services, Action<DbContextOptionsBuilder> dbContextOptions, Action<WebhookOptionsBuilder> webhookOptions)
    {
        var webhookOptionsBuilder = new WebhookOptionsBuilder();
        webhookOptions(webhookOptionsBuilder);

        services.AddSingleton(webhookOptionsBuilder.Build());
        services.AddDbContext<MinimalWebhooksDbContext>(dbContextOptions);
        services.AddTransient<IWebhookClientHttpClient, WebhookClientHttpClient>();
        services.AddTransient<IWebhookDataStore, WebhookDataStore>();
        services.AddTransient<IWebhookOptionsProcessor, WebhookOptionsProcessor>();
        services.AddTransient<IWebhookActionEventProcessor, WebhookActionEventProcessor>();
        services.AddTransient<WebhookClientManager>();
        services.AddTransient<WebhookEventsManager>();
        return services;
    }
}