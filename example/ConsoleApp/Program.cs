using ConsoleApp.Models;
using MinimalWebHooks.Core.Enum;
using MinimalWebHooks.Core.Managers;
using MinimalWebHooks.Core.Models;
using MinimalWebHooks.Core.Models.DbSets;

namespace ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var dependencyProvider = new DependencyProvider();
            var clientManager = dependencyProvider.GetWebhookClientManager();
            var eventsManager = dependencyProvider.GetWebhookEventsManager();

            await CreateNewCustomer(clientManager, eventsManager);
        }

        private static async Task CreateNewCustomer(WebhookClientManager clientManager, WebhookEventsManager eventsManager)
        {
            // Lets create a client to recieve updates about customers. The Entity type is the 'fullname' of the type. You can also pass it an object and call 'GetEntityTypeName()' to get the correct name.
            var createClientResult = await clientManager.Create(new WebhookClient
                {
                    Name = "Create Customer Client Example",
                    WebhookUrl = "https://webhook.site/13043ffc-b84f-4746-9c7c-eb93e502582d",
                    EntityTypeName = "ConsoleApp.Models.Customer",
                    ActionType = WebhookActionType.Create,
                    ClientHeaders = null,
                    Disabled = false,
                }
            );

            // assume we've created and saved this customer into our DB
            var newCustomer = new Customer {Id = 1, FullName = "JordanAlec"};

            // We've created the customer in our DB, lets raise an 'event' that can be picked up later to send the creation update to our client's webhook url
            // We create a new event by creating a new 'WebhookActionEvent' object and call .CreateEvent, passing in the data you want to send and the CRUD action
            await eventsManager.WriteEvent(new WebhookActionEvent().CreateEvent(newCustomer, WebhookActionType.Create));

            // This will send all events on the queue, written from 'WriteEvents' above.
            var sendEventResults = await eventsManager.SendEvents();
        }
    }
}