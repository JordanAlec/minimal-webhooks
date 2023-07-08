using MinimalWebHooks.Api.Extensions.Startup;

namespace MinimalWebHooks.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddMinimalWebhooksApi(dbContextOptions =>
            {
                
            }, webhookApiOptions =>
            {

            },
            webhookOptions =>
            {

            });
            var app = builder.Build();
            app.UseMinimalWebhooksApi();
            app.Run();
        }
    }
}
