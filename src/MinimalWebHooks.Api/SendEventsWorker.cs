using MinimalWebHooks.Core.Managers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MinimalWebHooks.Api.Models;

namespace MinimalWebHooks.Api;

public class SendEventsWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly double _delay;
    private readonly PeriodicTimer _timer;
    
    public SendEventsWorker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _delay = serviceProvider.GetRequiredService<WebhookEventsWorkerOptions>().TimerValue;
        _timer = new PeriodicTimer(TimeSpan.FromMilliseconds(_delay));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested && await _timer.WaitForNextTickAsync(stoppingToken))
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var eventsManager = scope.ServiceProvider.GetRequiredService<WebhookEventsManager>();
                var results = await eventsManager.SendEvents();
                var hasResults = results.Any();
            }
            await Task.Delay(Convert.ToInt32(_delay), stoppingToken);
        }
    }
}