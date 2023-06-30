namespace MinimalWebHooks.Api.Extensions.Startup;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddMinimalWebhooksApi(this IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddEndpointsApiExplorer();
        return services;
    }
}