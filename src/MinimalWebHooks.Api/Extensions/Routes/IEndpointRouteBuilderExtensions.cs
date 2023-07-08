using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinimalWebHooks.Core.Managers;
using MinimalWebHooks.Core.Models;

namespace MinimalWebHooks.Api.Extensions.Routes;

internal static class IEndpointRouteBuilderExtensions
{
    internal static IEndpointRouteBuilder MapMinimalWebhooksApiRoutes(this IEndpointRouteBuilder routerBuilder)
    {
        routerBuilder.MapGet("/webhooks/clients", [Authorize("WebhookClientPolicy")] async ([FromServices] WebhookClientManager manager) =>
        {
            var clients = await manager.Get();
            return Results.Ok(clients);
        }).WithName("getClients");

        routerBuilder.MapGet("/webhooks/clients/{id}", [Authorize("WebhookClientPolicy")] async ([FromRoute]int id, [FromServices] WebhookClientManager manager) =>
        {
            var client = await manager.Get(id);
            return client.Success ? Results.Ok(client) : Results.BadRequest(client);
        }).WithName("getClient");

        routerBuilder.MapPost("/webhooks/clients/", [Authorize("WebhookClientPolicy")] async ([FromBody] WebhookClient webhookClient, [FromServices] WebhookClientManager manager) =>
        {
            var client = await manager.Create(webhookClient);
            return client.Success ? Results.Ok(client) : Results.BadRequest(client);
        }).WithName("createClient");

        return routerBuilder;
    }
}