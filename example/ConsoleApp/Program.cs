using System.Net;
using ConsoleApp.Models;
using MinimalWebHooks.Core.Enum;
using MinimalWebHooks.Core.Extensions;
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

            // Update the 'webhookUrl' and pick one of the following actions: 'Create', 'Update', 'Delete'
            await ExecuteExample(clientManager, eventsManager, "https://webhook.site/13043ffc-b84f-4746-9c7c-eb93e502582d", "Create");
        }

        private static async Task<WebhookDataResult> CreateClientSubbedToCustomer(WebhookClientManager clientManager, string webhookClientName, string url, WebhookActionType type)
        {
            var createClientResult = await clientManager.Create(new WebhookClient
                {
                    Name = webhookClientName,
                    WebhookUrl = url,
                    EntityTypeName = "ConsoleApp.Models.Customer",
                    ActionType = type,
                    ClientHeaders = null,
                    Disabled = false,
                }
            );
            return createClientResult;
        }
        private static Customer CreateCustomer() => new() {Id = 1, FullName = "JordanAlec"};

        private static async Task ExampleAction(WebhookClientManager clientManager, WebhookEventsManager eventsManager, string clientName, string clientUrl, WebhookActionType type)
        {
            // Lets create a client to recieve updates about customers. The Entity type is the 'fullname' of the type. You can also pass it an object and call 'GetEntityTypeName()' to get the correct name.
            await CreateClientSubbedToCustomer(clientManager, clientName, clientUrl, type);

            // assume we've created and saved this customer into our DB
            var newCustomer = CreateCustomer();

            // We've created the customer in our DB, lets raise an 'event' that can be picked up later to send the our client's webhook url
            // We create a new event by creating a new 'WebhookActionEvent' object and call .Create, passing in the data you want to send and the CRUD action
            var actionEvent = await new WebhookActionEvent().Create(newCustomer, type);
            await eventsManager.WriteEvent(actionEvent.AddUdf("SentFrom", Dns.GetHostName())); // adding UDFs are entirely optional

            // This will send all events on the queue, written from 'WriteEvents' above.
            var sendEventResults = await eventsManager.SendEvents();
        }

        private static async Task ExecuteExample(WebhookClientManager clientManager, WebhookEventsManager eventsManager, string webhookUrl, string action)
        {
            var actions = new Dictionary<string, Func<Task>>(StringComparer.InvariantCultureIgnoreCase)
            {
                {
                    "Create",
                    () => ExampleAction(clientManager, eventsManager, "Create Customer Client Example", webhookUrl, WebhookActionType.Create)

                },
                {
                    "Update",
                    () => ExampleAction(clientManager, eventsManager, "Update Customer Client Example", webhookUrl, WebhookActionType.Update)

                },
                {
                    "Delete",
                    () => ExampleAction(clientManager, eventsManager, "Delete Customer Client Example", webhookUrl, WebhookActionType.Delete)

                }
            };

            if (actions.TryGetValue(action, out var exampleAction))
                await exampleAction!();
        }
    }
}