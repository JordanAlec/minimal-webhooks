# minimal-webhooks
Packages to help facilitate webhooks within your APIs
Please check and run the examples. Comments are provided within the examples for explanation purposes.

The meaning behind this package is to provide a basic implementation of webhooks that you can install and use in your existing APIs. This allows you to send CREATE, UPDATE and DELETE events for your models to a number of 'subscribed' clients.
As this is built on top of your existing applications and not a standalone solution performance should be a factor in whether to utilise this.
This ideal solution to utilise this for would be 'one-off' API projects or smaller projects where the number of 'subscribed' clients are minimal. 
For larger projects, with a number of different clients that span over a number of different nodes, it may be best to look into an API Gateway solution, such as Tyk.

## MinimalWebhooks.Core
Provides the core functionality to facilitate webhooks.
It is best to use the .Web package below as this provides endpoints to manage webhook clients but also has the ability to automatically send events periodically. You will have to manage this programatically if Core is used solo.

Features include:
- Dependency Injection throughout. 
- Extensions to facilitate dependency injection and service registration. See 'AddMinimalWebhooksCore' call against IServiceCollection.
- EF Core to store webhook clients and their headers. You can pass in whatever database provider you wish to use.
- Provides Managers to create various webhook clients, allow various headers when calling webhook urls, validation during creation, event raising on a 'Queue' via Channels and sending events in batches.
- Logging throughout using ILogger. Your configurable options for this are outside of this package.

## MinimalWebhooks.Web
Provides all the functionality of Core as well as endpoints to manage those clients.
It is highly recommended to use this package other the .Core package to make full use of features.

Features include:
- All of .Core features.
- Additional Extensions to register and configure Api specific features.
- Endpoints to get, create, update and disable clients.
- Configurable policy options to help you define how you want to protect those endpoints. This can be omited but anonymous access will be provided.
- Option to use a 'BackgroundService' that periodically send's events on the queue. This can be configurable in terms of how often that period is. The default is every 10 minutes.

## Events

When an event is raised it can be sent a number in one of two ways:
- Programatically: You can inject / request the WebhookEventsManager and call SendEvents(); This will look through all possible events, and send them to the correct 'subscribed' clients. The below 'ConsoleApp' example does this.
- BackgroundService: You can enable a worker service that periodically checks and sends events. This is only available within the .Web package. The below 'Api' example does this and sends events every 30 seconds.

The events are sent via a POST request to the webhook client's url. How that is sent (via JSON / XML) is determined via setup. An example of what an event will look like if sent by JSON is below:

```json
{
  "ActionType": 0,
  "EventTimestamp": "2023-07-18T10:54:26.6578166+01:00",
  "Entity": {
    "Id": 1,
    "FullName": "JordanAlec"
  },
  "EntityTypeName": "ConsoleApp.Models.Customer",
  "Source": "24.221.88.118",
  "Udfs": [
    {
        "key": "example key",
        "value": "example value"
    }
  ]
}
```

- ActionType is whether the event was raised from a CREATE (0), UPDATE (1) or DELETE (2) event.
- EventTimestamp is when the event was raised, not sent.
- Entity is the object data from the event. This is passed in when creating the event. This is the data that was raised from the above action type. For example, this entity above was created and this is the data that was created.
- EntityTypeName is the full name from the 'type' of object passed in from creating the event. It is generated from the full name of the type. It is the full namespace plus the type name.
- Source is the external IP address of host you install the package on. For example, your computer, your API, etc.
- Udfs is a list of 'user defined fields' that you can add to an event. You can pass in a list into the "Create()" call from an "WebhookActionEvent" or call "AddUdf" on "WebhookActionEvent" to add single udfs at a time.

Additional headers can be passed in that are configurable from a webhook client standpoint.
This could be things like:
- Authorization header
- A customer header you want to pass for verification purposes
- Any additional headers needed to make the POST request to your webhook url target.

## Examples

### Api
This example is an Api project that uses MinimalWebhooks.Web. This will also use MinimalWebhooks.Core.
When the Api project is started, you should see a browser open with the Swagger documentation.
The endpoints listed as "Alert", are specific to this Api project and serve as an example of what a 'typical' CRUD Api may look like.
The endpoints lists as "Api" are the Webhook endpoints. These endpoints use .NET's minimal Api's.

Within the Api's program.cs file you can configure various options in how MinimalWebhooks behave. To make sure all is setup you'll need to call 'AddMinimalWebhooksApi' to add the relevant services to your service collection. 
The 'AddMinimalWebhooksCore' is not needed as a seperate call as the 'AddMinimalWebhooksApi' calls this downstream.

The options to the call are as follows:

dbContextOptions (This argument is also expected in the .Core variant: AddMinimalWebhooksCore):
- You can pass in your dbContextOptions, such as enabling detailed errors and specifying what database provider you will use to store webhook clients. The package uses EF Core.

webhookApiOptions:
- webhookApiOptions.SetAuthorizationPolicy() - You can create a policy to protect the webhook endpoints. If omitted then anonymous access is allowed. If used make sure you call 'AddAuthentication' and provide your options as you will get an InvalidOperationException
- An example to the above is: webhookApiOptions.SetAuthorizationPolicy(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());
- webhookApiOptions.SetWorkerOptions(); - This allows you to enable a 'BackgroundService' that will periodically send any events on the queue. The timer is passed in as milliseconds.
- An example to the above is: webhookApiOptions.SetWorkerOptions(30000);
-  webhookApiOptions.EnableWorker(); - Is another example of enabling the 'BackgroundService' however the periodical check is every ten minutes. If you omit this or the above call, no worker is registered and you will need to send events yourself.

webhookOptions (This argument is also expected in the .Core variant: AddMinimalWebhooksCore):
- webhookOptions.WebhookUrlIsReachable(); - This will make a HEAD request to the webhook client, prior to its creation. If the HEAD request fails the client will not be created. If you omit this no check is done.
- It is highly advised to keep this call as events will be attempted to the webhook client's url and may throw an exception.
- webhookOptions.SetWebhookActionEventSerialiser(); - This lets you decide how the events are serialised and the event's media type. You can omit this if your happy with the default behaviour.
- The default behaviour to the above is to use JSON in the POST requests and to use System.Text.Json.JsonSerializer.Serialize(). If you want to use something else implement the interface 'IWebhookActionEventSerialiser' and pass it into the call.
- An example to the above is webhookOptions.SetWebhookActionEventSerialiser(new DefaultWebhookActionEventSerialiser()); - NOTE: DefaultWebhookActionEventSerialiser is defaulted. There is no need to pass DefaultWebhookActionEventSerialiser into the call. This is for example purposes only.
- Using the above you could send XML or use a 3rd party package for the serialisation.
- webhookOptions.SetEventOptions(); - This lets you configure the channel used to create the queue to send events. If this is omited then the default behaviour has a capacity of 10, and will wait for space to be available if at capacity.
- The capacity is also used to partially send events. If your capacity is 10, events will also be sent in groups of 10.
- An example to the above is webhookOptions.SetEventOptions(1, BoundedChannelFullMode.DropOldest);

A further call on the 'WebApplication' is used called: 'UseMinimalWebhooksApi()'. No options are passed. This is used to add the following calls:
- UseHttpsRedirection
- UseAuthorizationg
- And also map the endpoints to GET, POST, PATCH, DELETE webhook clients. DELETE only disables clients. They can be re-enabled using the PATCH endpoint.

### ConsoleApp
This example is a console app that uses MinimalWebhooks.Core only.
To do this, check the 'DependencyProvider' class within the project that calls 'AddMinimalWebhooksCore'.
Since this isn't an API and doesn't have logging as an implicit reference the 'AddLogging' call is made to resolve ILogger through the MinimalWebhooks.Core's Managers.

Because this is Core only, a lot of tasks will be done programatically, such as creating clients, writing and sending events.

To run this:
- Visit the 'Program.cs' file
- Review the 'CreateNewCustomer' static method
- Insert the WebhookUrl you want to be called. You can use 'https://webhook.site/' as a testing tool.
- If you want to adjust the client in anyway you can, for example adding new key, value pairs for the client headers. An example is below

```csharp
var createClientResult = await clientManager.Create(new WebhookClient
    {
        Name = "Create Customer Client Example",
        WebhookUrl = "https://webhook.site/13043ffc-b84f-4746-9c7c-eb93e502582d",
        EntityTypeName = "ConsoleApp.Models.Customer",
        ActionType = WebhookActionType.Create,
        ClientHeaders = new List<WebhookClientHeader>
        {
            new WebhookClientHeader
            {
                Key = "SuperSecretKey",
                Value = "SuperSecretValue"
            }
        },
        Disabled = false,
    });
```

The logging should assist to identify issues.
Common issues include:
- The WebhookUrl not being set or that the URL cannot recieve a HEAD request. You can disable this check by removing the 'webhookOptions.WebhookUrlIsReachable()' call.
- The WebhookActionType not matching the client to the event. All events are sent by looking through the clients that have subscribed to a particular action type.
- The EntityTypeName not matching the 'data' passed into the event. The 'Create' call is generic. The EntityTypeName must match the full type name or you can call 'GetEntityTypeName' on any object.
- Disabled flag on the client is set to 'true'. Not events are passed to disabled clients.

### Admin
This example is a typescript next.js application that calls the [Api](#api) example.
Features include: getting list of webhook clients, viewing client details, viewing activity logs, creating new clients, disabling / re-enabling clients.
This example is currently incomplete and was created to show an example of how to create a front-end to management webhook clients. 

Further notes:

- If you are not running the [Api](#api) example project you will see alerts erroring when going to the various clients page. 
- If you want to adjust the Api url it calls adjust the "WEBHOOKS_API_URL" environment variable within the .env.development file.
- NODE_TLS_REJECT_UNAUTHORIZED = '0' is used as it's assumed your running the Api and Admin example locally.

The focus of this example is to provide a [demo frontend for an (largely) api only package](https://www.thoughtworks.com/radar/techniques/demo-frontends-for-api-only-products). As such this example is not meant to be extensive and/or exhaustive.
There is no security configured within the front end and assumes the webhooks api endpoints can be accessed anonymously. Because of this it is not recommended to use and deploy this front end "as is", but feel free to use this or any examples as a basis for your own applications.