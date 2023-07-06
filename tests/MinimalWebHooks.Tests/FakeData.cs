using Bogus;
using MinimalWebHooks.Core.Enum;
using MinimalWebHooks.Core.Extensions;

namespace MinimalWebHooks.Tests;

public static class FakeData
{
    private static Faker<WebhookClient> FakeWebhookClient(WebhookActionType? actionType = null) =>
        new Faker<WebhookClient>()
            .RuleFor(x => x.Id, f => f.Random.Int(min: 1))
            .RuleFor(x => x.Name, f => f.Name.FullName())
            .RuleFor(x => x.WebhookUrl, f => f.Internet.Url())
            .RuleFor(x => x.ActionType, f => actionType ?? f.PickRandom<WebhookActionType>())
            .RuleFor(x => x.EntityTypeName, f => f.Random.Word());

    public static WebhookClient WebhookClient(WebhookActionType? actionType = null) => FakeWebhookClient(actionType);

    public static List<WebhookClient> WebhookClients(int generate,  WebhookActionType? actionType = null) => FakeWebhookClient(actionType).Generate(generate);

    private static Faker<WebhookActionEvent> FakeWebhookActionEvent<T>(T data, WebhookActionType? actionType = null) =>
        new Faker<WebhookActionEvent>()
            .RuleFor(x => x.ActionType, f => actionType ?? f.PickRandom<WebhookActionType>())
            .RuleFor(x => x.EventTimestamp, f => f.Date.Recent())
            .RuleFor(x => x.Entity, data)
            .RuleFor(x => x.EntityTypeName, data.GetEntityTypeName());

    public static WebhookActionEvent WebhookActionEvent<T>(T data, WebhookActionType? actionType = null) => FakeWebhookActionEvent(data, actionType);

    public static List<WebhookActionEvent> WebhookActionEvents<T>(T data, int generate, WebhookActionType? actionType = null) => FakeWebhookActionEvent(data, actionType).Generate(generate);
}