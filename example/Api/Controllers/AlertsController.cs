using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using MinimalWebHooks.Core.Enum;
using MinimalWebHooks.Core.Managers;
using MinimalWebHooks.Core.Models;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlertsController : ControllerBase
    {
        private readonly ILogger<AlertsController> _logger;
        private readonly WebhookEventsManager _manager;

        public AlertsController(ILogger<AlertsController> logger, WebhookEventsManager manager)
        {
            _logger = logger;
            _manager = manager; // inject the events manager whenever needed.
        }

        [HttpGet]
        public async Task<IActionResult> Get() =>
            InMemoryAlerts.Alerts.Any() ? Ok(InMemoryAlerts.Alerts) : BadRequest();

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Alert alert)
        {
            // Lets assume this basic POST example for alerts is in your API.
            _logger.LogInformation($"CREATE request for alert");
            if (alert.Id <= 0) return BadRequest(new { status = $"Alert id needs to be greater than 0: {alert.Id}" });
            InMemoryAlerts.Alerts.TryGetValue(alert.Id, out var foundAlert);
            if (foundAlert != null) return BadRequest(new {status = $"Alert already exists with id: {alert.Id}"});
            var result = InMemoryAlerts.Alerts.TryAdd(alert.Id, alert);
            // If the alert was successfully created we'll create an event to say its created.
            // This will send the alert put a message on an internal queue ready to be sent by the worker (if enabled) or by yourself at a later date.
            if (result) await _manager.WriteEvent(await new WebhookActionEvent().Create(alert, WebhookActionType.Create));
            return result ? Ok(result) : BadRequest(new { status = $"Failed to create alert" });
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            // Lets assume this basic DELETE example for alerts is in your API.
            _logger.LogInformation($"DELETE request for alert: {id}");
            InMemoryAlerts.Alerts.TryGetValue(id, out var foundAlert);
            if (foundAlert == null) return BadRequest(new { status = $"Alert doesnt exists with id: {id}" });
            var result = InMemoryAlerts.Alerts.Remove(id);
            // If the alert was successfully deleted we'll create an event to say its deleted.
            // This will send the alert put a message on an internal queue ready to be sent by the worker (if enabled) or by yourself at a later date.
            if (result) await _manager.WriteEvent(await new WebhookActionEvent().Create(foundAlert, WebhookActionType.Delete));
            return result ? Ok(new { status = $"Successfully deleted alert" }) : BadRequest(new { status = $"Failed to delete alert" });
        }
    }
}