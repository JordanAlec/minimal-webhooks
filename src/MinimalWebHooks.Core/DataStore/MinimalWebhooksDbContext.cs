using Microsoft.EntityFrameworkCore;
using MinimalWebHooks.Core.Models;

namespace MinimalWebHooks.Core.DataStore
{
    public class MinimalWebhooksDbContext : DbContext
    {
        public DbSet<WebhookClient> WebhookClients { get; set; }

        public MinimalWebhooksDbContext(DbContextOptions<MinimalWebhooksDbContext> options) : base(options)
        {}
    }
}