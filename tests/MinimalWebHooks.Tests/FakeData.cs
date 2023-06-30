using Bogus;

namespace MinimalWebHooks.Tests;

public static class FakeData
{
    private static Faker<WebhookClient> FakeWebhookClient() =>
        new Faker<WebhookClient>()
            .RuleFor(x => x.Id, f => f.Random.Int(min: 1))
            .RuleFor(x => x.Name, f => f.Name.FullName());

    public static WebhookClient WebhookClient() => FakeWebhookClient();

    public static List<WebhookClient> WebhookClients(int generate) => FakeWebhookClient().Generate(generate);
}