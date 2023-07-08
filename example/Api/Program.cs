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
                webhookOptions =>
            {
                webhookOptions.WebhookUrlIsReachable();

                // This is not needed but if you want to send XML, use a 3rd party Json Serialiser you can. This is the default behaviour.
                // If you want to create your own create an object that implements the 'IWebhookActionEventSerialiser' interface. 
                webhookOptions.SetWebhookActionEventSerialiser(new DefaultWebhookActionEventSerialiser());
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