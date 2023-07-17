using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MinimalWebHooks.Core.Builders;
using MinimalWebHooks.Core.DataStore;
using MinimalWebHooks.Core.Http;
using MinimalWebHooks.Core.Interfaces;
using MinimalWebHooks.Core.Managers;
using MinimalWebHooks.Core.Processors;
using MinimalWebHooks.Core.Serialisation;

namespace MinimalWebHooks.Core.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddMinimalWebhooksCore(this IServiceCollection services, Action<DbContextOptionsBuilder> dbContextOptions, Action<WebhookOptionsBuilder> webhookOptions)
    {
        var webhookOptionsBuilder = new WebhookOptionsBuilder();
        webhookOptions(webhookOptionsBuilder);
        var builtOptions = webhookOptionsBuilder.Build();
        if (builtOptions.EventSerialiser == null) 
            builtOptions = webhookOptionsBuilder.SetWebhookActionEventSerialiser(new DefaultWebhookActionEventSerialiser()).Build();

        services.AddSingleton(builtOptions);
        services.AddDbContext<MinimalWebhooksDbContext>(dbContextOptions);
        services.AddTransient<IWebhookClientHttpClient, WebhookClientHttpClient>();
        services.AddTransient<IWebhookDataStore, WebhookDataStore>();
        services.AddTransient<IWebhookOptionsProcessor, WebhookOptionsProcessor>();
        services.AddSingleton<IWebhookActionEventProcessor, WebhookActionEventProcessor>();
        services.AddTransient<WebhookClientManager>();
        services.AddTransient<WebhookEventsManager>();
        return services;
    }
}