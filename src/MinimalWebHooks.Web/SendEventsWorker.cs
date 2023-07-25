using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MinimalWebHooks.Core.Managers;
using MinimalWebHooks.Web.Models;

namespace MinimalWebHooks.Web;

public class SendEventsWorker : BackgroundService
{
    private readonly ILogger<SendEventsWorker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly double _delay;
    private readonly PeriodicTimer _timer;
    
    public SendEventsWorker(ILogger<SendEventsWorker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _delay = serviceProvider.GetRequiredService<WebhookEventsWorkerOptions>().TimerValue;
        _timer = new PeriodicTimer(TimeSpan.FromMilliseconds(_delay));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested && await _timer.WaitForNextTickAsync(stoppingToken))
        {
            _logger.LogDebug("{logger}: STARTED", nameof(SendEventsWorker));
            using (var scope = _serviceProvider.CreateScope())
            {
                var eventsManager = scope.ServiceProvider.GetRequiredService<WebhookEventsManager>();
                await eventsManager.SendEvents();
            }
            _logger.LogDebug("{logger}: FINISHED", nameof(SendEventsWorker));
            await Task.Delay(Convert.ToInt32(_delay), stoppingToken);
        }
    }
}