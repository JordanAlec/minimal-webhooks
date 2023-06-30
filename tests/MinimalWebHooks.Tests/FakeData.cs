using Bogus;

namespace MinimalWebHooks.Tests;

public static class FakeData
{
    private static Faker<WebhookClient> FakeWebhookClient(bool emptyWebhookUrl = false) =>
        new Faker<WebhookClient>()
            .RuleFor(x => x.Id, f => f.Random.Int(min: 1))
            .RuleFor(x => x.Name, f => f.Name.FullName())
            .RuleFor(x => x.WebhookUrl, f => emptyWebhookUrl ? string.Empty : f.Internet.Url());

    public static WebhookClient WebhookClient(bool emptyWebhookUrl = false) => FakeWebhookClient();

    public static List<WebhookClient> WebhookClients(int generate, bool emptyWebhookUrl = false) => FakeWebhookClient().Generate(generate);
}