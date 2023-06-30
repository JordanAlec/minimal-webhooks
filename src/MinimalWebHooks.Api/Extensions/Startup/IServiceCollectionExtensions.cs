using Microsoft.EntityFrameworkCore;
using MinimalWebHooks.Core.Extensions;

namespace MinimalWebHooks.Api.Extensions.Startup;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddMinimalWebhooksApi(this IServiceCollection services, Action<DbContextOptionsBuilder> dbContextOptions)
    {
        services.AddAuthorization();
        services.AddEndpointsApiExplorer();
        services.AddMinimalWebhooksCore(dbContextOptions);
        return services;
    }
}