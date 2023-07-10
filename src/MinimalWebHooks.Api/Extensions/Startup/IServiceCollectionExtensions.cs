using Microsoft.AspNetCore.Authorization;
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
            var policy = builtOptions.AuthPolicy ?? new AuthorizationPolicyBuilder().RequireAssertion(context => true).Build();
            options.AddPolicy(ApiConstants.Policy.WebhookPolicyName, policy);
        });

        services.AddMinimalWebhooksCore(dbContextOptions, webhookOptions);
        return services;
    }
}