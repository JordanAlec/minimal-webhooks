using Bogus;
using MinimalWebHooks.Core.Enum;
using MinimalWebHooks.Core.Extensions;
using MinimalWebHooks.Core.Models.DbSets;

namespace MinimalWebHooks.Tests;

public static class FakeData
{
    private static Faker<WebhookClient> FakeWebhookClient(WebhookActionType? actionType = null) =>
        new Faker<WebhookClient>()
            .RuleFor(x => x.Id, f => f.Random.Int(min: 1))
            .RuleFor(x => x.Name, f => f.Name.FullName())
            .RuleFor(x => x.WebhookUrl, f => f.Internet.Url())
            .RuleFor(x => x.ActionType, f => actionType ?? f.PickRandom<WebhookActionType>())
            .RuleFor(x => x.EntityTypeName, f => f.Random.Word())
            .RuleFor(x => x.Disabled, false)
            .RuleFor(x => x.ClientHeaders, f => WebhookClientHeaders(1));

    public static WebhookClient WebhookClient(WebhookActionType? actionType = null) => FakeWebhookClient(actionType);

    public static List<WebhookClient> WebhookClients(int generate,  WebhookActionType? actionType = null) => FakeWebhookClient(actionType).Generate(generate);

    private static Faker<WebhookActionEvent> FakeWebhookActionEvent<T>(T data, WebhookActionType? actionType = null) =>
        new Faker<WebhookActionEvent>()
            .RuleFor(x => x.ActionType, f => actionType ?? f.PickRandom<WebhookActionType>())
            .RuleFor(x => x.EventTimestamp, f => f.Date.Recent())
            .RuleFor(x => x.Entity, data)
            .RuleFor(x => x.EntityTypeName, data.GetEntityTypeName())
            .RuleFor(x => x.Source, f => f.Internet.Ip());

    public static WebhookActionEvent WebhookActionEvent<T>(T data, WebhookActionType? actionType = null) => FakeWebhookActionEvent(data, actionType);

    public static List<WebhookActionEvent> WebhookActionEvents<T>(T data, int generate, WebhookActionType? actionType = null) => FakeWebhookActionEvent(data, actionType).Generate(generate);

    public static WebhookUpdateCommand WebhookUpdateCommand(int id, bool disabledFlag, bool deleteHeaders, Dictionary<string, string>? headers = null) =>
        new Faker<WebhookUpdateCommand>()
            .RuleFor(x => x.Id, id)
            .RuleFor(x => x.SetDisabledFlag, disabledFlag)
            .RuleFor(x => x.WebhookUrl, f => f.Internet.Url())
            .RuleFor(x => x.DeleteAllHeaders, deleteHeaders)
            .RuleFor(x => x.ReplaceHeaders, f =>
            {
                var data = new List<WebhookClientHeader>();
                if (headers != null) data.AddRange(headers.Select(WebhookClientHeader));
                return data;
            });

    private static WebhookClientHeader WebhookClientHeader(KeyValuePair<string, string> keyValue) =>
        new Faker<WebhookClientHeader>()
            .RuleFor(x => x.Id, f => f.Random.Int(min: 1))
            .RuleFor(x => x.Key, keyValue.Key)
            .RuleFor(x => x.Value, keyValue.Value);

    private static List<WebhookClientHeader> WebhookClientHeaders(int generate) =>
        new Faker<WebhookClientHeader>()
            .RuleFor(x => x.Id, f => f.Random.Int(min: 1))
            .RuleFor(x => x.Key, f => f.Random.Word())
            .RuleFor(x => x.Value, f => f.Random.Word())
            .Generate(generate);
}