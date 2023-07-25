using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinimalWebHooks.Core.Extensions;
using MinimalWebHooks.Core.Managers;
using System.Threading.Channels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ConsoleApp;

public class DependencyProvider
{
    private readonly ServiceProvider _serviceProvider;
    public DependencyProvider()
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection, BuildConfiguration());
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    public WebhookClientManager GetWebhookClientManager() => _serviceProvider.GetService<WebhookClientManager>();
    public WebhookEventsManager GetWebhookEventsManager() => _serviceProvider.GetService<WebhookEventsManager>();

    private IConfiguration BuildConfiguration()
    {
        return new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();
    }
    private void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddLogging(options =>
        {
            options.SetMinimumLevel(LogLevel.Information);
            options.AddConsole();
        });

        services.AddMinimalWebhooksCore(dbContextOptions =>
        {
            dbContextOptions.UseInMemoryDatabase("ConsoleAppExample");
        },
        webhookOptions =>
        {
            webhookOptions.WebhookUrlIsReachable();
            webhookOptions.SetEventOptions(5, BoundedChannelFullMode.DropOldest);
        });
    }
}