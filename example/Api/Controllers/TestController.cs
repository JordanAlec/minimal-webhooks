using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using MinimalWebHooks.Core.Enum;
using MinimalWebHooks.Core.Extensions;
using MinimalWebHooks.Core.Managers;
using MinimalWebHooks.Core.Models;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly WebhookClientManager _clientsManager;
        private readonly WebhookEventsManager _eventsManager;

        public TestController(ILogger<TestController> logger, WebhookClientManager clientsManager, WebhookEventsManager eventsManager)
        {
            _logger = logger;
            _clientsManager = clientsManager;
            _eventsManager = eventsManager;
        }


        [HttpGet]
        [Route("ExampleClientCreation/{webhookUrl}")]
        public async Task<IActionResult> Create([FromRoute] string webhookUrl)
        {
            // Assume we hit a 'POST' endpoint to create this below 'Alert'.
            var alert = new Alert { Id = 1, Status = "Alert", Title = "Super important" };

            // Lets create a webhook in code (you can do this via an API call as well) and it'll save into your datastore.
            var clientExample = new WebhookClient {Id = 1, ActionType = WebhookActionType.Create, EntityTypeName = alert.GetEntityTypeName(), WebhookUrl = webhookUrl, Name = "Alert Example Client"};
            var clientCreationResult = await _clientsManager.Create(clientExample);

            // When you want to raise an 'Event' to send the webhook clients you write an event like below. This will only be sent to client that subscribe to the 'EntityTypeName' for 'Alert' and for 'Create' actions:
            var writeEventResult = await _eventsManager.WriteEvent(new WebhookActionEvent().CreateEvent(alert, WebhookActionType.Create));

            // Once written events you can send events in bulk, like below:
            var sendEventResults = await _eventsManager.SendEvents();

            return Ok(sendEventResults);
        }
    }
}
