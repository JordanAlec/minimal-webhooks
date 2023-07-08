using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlertsController : ControllerBase
    {
        private readonly ILogger<AlertsController> _logger;

        public AlertsController(ILogger<AlertsController> logger) => _logger = logger;

        [HttpGet]
        public async Task<IActionResult> Get() =>
            InMemoryAlerts.Alerts.Any() ? Ok(InMemoryAlerts.Alerts) : BadRequest();

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Alert alert)
        {
            _logger.LogInformation($"CREATE request for alert");
            if (alert.Id <= 0) return BadRequest(new { status = $"Alert id needs to be greater than 0: {alert.Id}" });
            InMemoryAlerts.Alerts.TryGetValue(alert.Id, out var foundAlert);
            if (foundAlert != null) return BadRequest(new {status = $"Alert already exists with id: {alert.Id}"});
            var result = InMemoryAlerts.Alerts.TryAdd(alert.Id, alert);
            return result ? Ok(result) : BadRequest(new { status = $"Failed to create alert" });
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            _logger.LogInformation($"DELETE request for alert: {id}");
            InMemoryAlerts.Alerts.TryGetValue(id, out var foundAlert);
            if (foundAlert == null) return BadRequest(new { status = $"Alert doesnt exists with id: {id}" });
            var result = InMemoryAlerts.Alerts.Remove(id);
            return result ? Ok(new { status = $"Successfully deleted alert" }) : BadRequest(new { status = $"Failed to delete alert" });
        }
    }
}