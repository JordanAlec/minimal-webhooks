using System.Threading.Channels;
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
                // webhookApiOptions.SetAuthorizationPolicy(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());

                // If you want to leave it anonymous then do not set the policy.

                // This will enable a worker that will automatically send events periodically. The value is in milliseconds. 1000 = 1 second.
                webhookApiOptions.SetWorkerOptions(1000);

                // If you dont want the default of ten minutes you can call this instead.
                // If you call both the last call will overwrite previous ones.
                // webhookApiOptions.EnableWorker();
            },
            webhookOptions =>
            {
                webhookOptions.WebhookUrlIsReachable();

                // You can choose how you want to send data to the webhook URL. The 'IWebhookActionEventSerialiser' interface lets you define the media type and how Events are serialised.
                // You can choose to omit the call if you want the defaults.
                // The default behaviour is to use JSON and use System.Text.Json.JsonSerializer.Serialize();
                // If you want to create your own create an object that implements the 'IWebhookActionEventSerialiser' interface. 
                // webhookOptions.SetWebhookActionEventSerialiser(new DefaultWebhookActionEventSerialiser());

                // You can choose the capacity options for events waiting to be sent to various webhook client urls.
                // You can choose to omit the call if you want the defaults.
                // the default behaviour has a capacity of 10, and will wait for space to be available if at capacity.
                webhookOptions.SetEventOptions(1, BoundedChannelFullMode.Wait);
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