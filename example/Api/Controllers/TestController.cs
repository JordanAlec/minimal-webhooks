using Api.Models;
using Microsoft.AspNetCore.Mvc;
using MinimalWebHooks.Core.Enum;
using MinimalWebHooks.Core.Extensions;
using MinimalWebHooks.Core.Managers;
using MinimalWebHooks.Core.Models;
using MinimalWebHooks.Core.Models.DbSets;

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
        [Route("ExampleClientCreation")]
        public async Task<IActionResult> Create()
        {
            // Assume we hit a 'POST' endpoint to create this below 'Alert'.
            var alert = new Alert { Id = 10, Status = "Alert", Title = "Super important" };

            // Lets create a webhook in code (you can do this via an API call as well) and it'll save into your datastore.
            var clientExample = new WebhookClient
            {
                Id = 1, 
                ActionType = WebhookActionType.Create, 
                EntityTypeName = alert.GetEntityTypeName(), 
                WebhookUrl = "", 
                Name = "Alert Example Client", 
                ClientHeaders = new List<WebhookClientHeader>{ new() {Id = 1, Key = "Authorization", Value = "Basic dXNlcm5hbWU6cGFzc3dvcmQ=" }, new() { Id = 2, Key = "Secret", Value = Guid.NewGuid().ToString() } }
            };

            var clientCreationResult = await _clientsManager.Create(clientExample);

            // When you want to raise an 'Event' to send the webhook clients you write an event like below. This will only be sent to client that subscribe to the 'EntityTypeName' for 'Alert' and for 'Create' actions:
            await _eventsManager.WriteEvent(new WebhookActionEvent().CreateEvent(alert, WebhookActionType.Create));

            // Once written events you can send events in bulk, like below
            // The event 'WebhookActionEvent' is only POST'ed to the webhook URL not the return value from 'WriteEvent'. If you want to find the client that sent the event, add a unique header to the client for identification.
            var sendEventResults = await _eventsManager.SendEvents();

            return Ok(sendEventResults);
        }
    }
}
