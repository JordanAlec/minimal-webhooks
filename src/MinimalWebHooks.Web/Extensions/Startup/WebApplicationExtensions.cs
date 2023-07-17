using Microsoft.AspNetCore.Builder;
using MinimalWebHooks.Web.Extensions.Routes;

namespace MinimalWebHooks.Web.Extensions.Startup;

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