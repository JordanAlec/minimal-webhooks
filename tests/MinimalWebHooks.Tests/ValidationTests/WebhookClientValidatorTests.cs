using MinimalWebHooks.Core.Validation.Rules;
using MinimalWebHooks.Core.Validation;
using MinimalWebHooks.Core.Models.DbSets;

namespace MinimalWebHooks.Tests.ValidationTests;

public class WebhookClientValidatorTests
{
    private async Task TestValidationResult(WebhookClient client, MockWebhookOptionsProcessorBuilder optionsBuilder, bool isValid, string message)
    {
        var validator = new WebhookClientValidator(client, new List<IValidationRule>
        {
            new WebhookClientHasRequiredProps(client),
            new WebhookClientUrlCanBeReached(client, optionsBuilder.Build().Object)
        });

        var result = await validator.ValidateClient();

        result.ValidatedClient.Should().BeEquivalentTo(client);
        result.IsValid().Should().Be(isValid);
        result.GetMessage().Should().Be(message);
    }

    [Fact]
    public async Task ValidationSuccesful()
    {
        var client = FakeData.WebhookClient();
        var optionsBuilder = new MockWebhookOptionsProcessorBuilder().SetupVerifyWebhookUrl(client, true);
        await TestValidationResult(client, optionsBuilder, true, "Passed validation: WebhookClientHasRequiredProps. Client 'WebhookUrl' has been verified with a HEAD request or 'WebhookUrlIsReachable' has not been set.");
    }

    [Fact]
    public async Task ValidationFailureEmptyEntityTypeName()
    {
        var client = FakeData.WebhookClient();
        client.EntityTypeName = string.Empty;
        var optionsBuilder = new MockWebhookOptionsProcessorBuilder().SetupVerifyWebhookUrl(client, true);
        await TestValidationResult(client, optionsBuilder, false, "Client must have 'EntityTypeName'. Client 'WebhookUrl' has been verified with a HEAD request or 'WebhookUrlIsReachable' has not been set.");
    }

    [Fact]
    public async Task ValidationFailureEmptyWebhookUrl()
    {
        var client = FakeData.WebhookClient();
        client.WebhookUrl = string.Empty;
        var optionsBuilder = new MockWebhookOptionsProcessorBuilder().SetupVerifyWebhookUrl(client, true);
        await TestValidationResult(client, optionsBuilder, false, "Client must have 'WebhookUrl'. Client 'WebhookUrl' has been verified with a HEAD request or 'WebhookUrlIsReachable' has not been set.");
    }

    [Fact]
    public async Task ValidationFailureEmptyName()
    {
        var client = FakeData.WebhookClient();
        client.Name = string.Empty;
        var optionsBuilder = new MockWebhookOptionsProcessorBuilder().SetupVerifyWebhookUrl(client, true);
        await TestValidationResult(client, optionsBuilder, false, "Client must have 'Name'. Client 'WebhookUrl' has been verified with a HEAD request or 'WebhookUrlIsReachable' has not been set.");
    }

    [Fact]
    public async Task ValidationFailureDisabledClient()
    {
        var client = FakeData.WebhookClient();
        client.Disabled = true;
        var optionsBuilder = new MockWebhookOptionsProcessorBuilder().SetupVerifyWebhookUrl(client, true);
        await TestValidationResult(client, optionsBuilder, false, "Client must have 'Disabled' set as false. Client 'WebhookUrl' has been verified with a HEAD request or 'WebhookUrlIsReachable' has not been set.");
    }

    [Fact]
    public async Task ValidationFailureCannotVerifyUrl()
    {
        var client = FakeData.WebhookClient();
        var optionsBuilder = new MockWebhookOptionsProcessorBuilder().SetupVerifyWebhookUrl(client, false);
        await TestValidationResult(client, optionsBuilder, false, "Passed validation: WebhookClientHasRequiredProps. Cannot verify client 'WebhookUrl'. Make sure the URL can receive a HEAD request or do not set 'WebhookUrlIsReachable'.");
    }
}