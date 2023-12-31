using System.Threading.Channels;
using Microsoft.EntityFrameworkCore;
using MinimalWebHooks.Web.Extensions.Startup;

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
                dbContextOptions.UseSqlite("Data Source=MinimalWebhooks.db;", b => b.MigrationsAssembly("Api"));
                dbContextOptions.EnableDetailedErrors();
                dbContextOptions.EnableSensitiveDataLogging();
            },
            webhookApiOptions =>
            {
                webhookApiOptions.SetWorkerOptions(30000);
            },
            webhookOptions =>
            {
                webhookOptions.WebhookUrlIsReachable();
                webhookOptions.SetEventOptions(1, BoundedChannelFullMode.DropOldest);
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