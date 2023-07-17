using Microsoft.AspNetCore.Builder;
using MinimalWebHooks.Api.Extensions.Routes;

namespace MinimalWebHooks.Api.Extensions.Startup;

public static class WebApplicationExtensions
{
    public static WebApplication UseMinimalWebhooksApi(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapMinimalWebhooksApiRoutes();
        return app;
    }
}