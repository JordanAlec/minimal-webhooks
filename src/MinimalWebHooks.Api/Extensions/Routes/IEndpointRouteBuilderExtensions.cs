namespace MinimalWebHooks.Api.Extensions.Routes;

internal static class IEndpointRouteBuilderExtensions
{
    internal static IEndpointRouteBuilder MapMinimalWebhooksApiRoutes(this IEndpointRouteBuilder routerBuilder)
    {
        routerBuilder.MapGet("/webhooks/home", (HttpContext httpContext) => Results.Ok(new {status = true, timeStamp = DateTime.Now}))
                    .WithName("webhooksHome");

        return routerBuilder;
    }
}