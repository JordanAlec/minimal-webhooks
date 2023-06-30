using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MinimalWebHooks.Core.DataStore;
using MinimalWebHooks.Core.Interfaces;
using MinimalWebHooks.Core.Managers;

namespace MinimalWebHooks.Core.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddMinimalWebhooksCore(this IServiceCollection services, Action<DbContextOptionsBuilder> dbContextOptions)
    {
        services.AddDbContext<MinimalWebhooksDbContext>(dbContextOptions);
        services.AddTransient<IWebhookDataStore, WebhookDataStore>();
        services.AddTransient<WebhookClientManager>();
        return services;
    }
}