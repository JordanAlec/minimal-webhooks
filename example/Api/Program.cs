using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MinimalWebHooks.Api.Extensions.Startup;
using MinimalWebHooks.Core.Serialisation;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMinimalWebhooksApi(dbContextOptions =>
            {
                dbContextOptions.UseInMemoryDatabase("MinimalWebhooksDb");
                dbContextOptions.EnableDetailedErrors();
                dbContextOptions.EnableSensitiveDataLogging();
            },
            webhookApiOptions =>
            {
                // This allows you to mark the endpoints as anonymous or you can create whatever policy you'd like to protect the GET / CREATE / DELETE webhook client endpoints.

                // To set a genuine policy like below make sure you 'AddAuthentication' to your IServiceCollection.
                webhookApiOptions.SetAuthorizationPolicy(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());

                // If you want to leave it anonymous then do not set the policy.
            },
            webhookOptions =>
            {
                webhookOptions.WebhookUrlIsReachable();

                // You can choose how you want to send data to the webhook URL. The 'IWebhookActionEventSerialiser' interface lets you define the media type and how Events are serialised.
                // The default behaviour is to use JSON and use System.Text.Json.JsonSerializer.Serialize();
                // If you want to create your own create an object that implements the 'IWebhookActionEventSerialiser' interface. 
                // webhookOptions.SetWebhookActionEventSerialiser(new DefaultWebhookActionEventSerialiser());
            });

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.UseMinimalWebhooksApi();

            app.Run();
        }
    }
}