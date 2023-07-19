using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MinimalWebHooks.Core.Managers;
using MinimalWebHooks.Core.Models;
using MinimalWebHooks.Core.Models.DbSets;

namespace MinimalWebHooks.Web.Extensions.Routes;

internal static class IEndpointRouteBuilderExtensions
{
    internal static IEndpointRouteBuilder MapMinimalWebhooksApiRoutes(this IEndpointRouteBuilder routerBuilder)
    {
        routerBuilder.MapGet("/webhooks/clients", [Authorize(ApiConstants.Policy.WebhookPolicyName)] async ([FromServices] WebhookClientManager manager) =>
        {
            var clients = await manager.Get();
            return Results.Ok(clients);
        }).WithName("getClients");

        routerBuilder.MapGet("/webhooks/clients/{id}", [Authorize(ApiConstants.Policy.WebhookPolicyName)] async ([FromRoute]int id, [FromServices] WebhookClientManager manager) =>
        {
            var client = await manager.Get(id, false);
            return client.Success ? Results.Ok(client) : Results.BadRequest(client);
        }).WithName("getClient");

        routerBuilder.MapPost("/webhooks/clients/", [Authorize(ApiConstants.Policy.WebhookPolicyName)] async ([FromBody] WebhookClient webhookClient, [FromServices] WebhookClientManager manager) =>
        {
            var client = await manager.Create(webhookClient);
            return client.Success ? Results.Ok(client) : Results.BadRequest(client);
        }).WithName("createClient");

        routerBuilder.MapDelete("/webhooks/clients/{id}", [Authorize(ApiConstants.Policy.WebhookPolicyName)] async ([FromRoute] int id, [FromServices] WebhookClientManager manager) =>
        {
            var client = await manager.Disable(id);
            return client.Success ? Results.Ok(client) : Results.BadRequest(client);
        }).WithName("disableClient");

        routerBuilder.MapPatch("/webhooks/clients", [Authorize(ApiConstants.Policy.WebhookPolicyName)] async ([FromBody] WebhookUpdateCommand command, [FromServices] WebhookClientManager manager) =>
        {
            var client = await manager.Update(command);
            return client.Success ? Results.Ok(client) : Results.BadRequest(client);
        }).WithName("updateClient");

        return routerBuilder;
    }

    // This is needed due to the downgrade to .NET 6 (until .NET 8)
    internal static RouteHandlerBuilder MapPatch(this IEndpointRouteBuilder endpoints, string pattern, Delegate handler) => endpoints.MapMethods(pattern, new[] { "PATCH" }, handler);
}