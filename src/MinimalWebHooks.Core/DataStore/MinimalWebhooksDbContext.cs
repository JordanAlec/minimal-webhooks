using Microsoft.EntityFrameworkCore;
using MinimalWebHooks.Core.Models.DbSets;

namespace MinimalWebHooks.Core.DataStore
{
    public class MinimalWebhooksDbContext : DbContext
    {
        public DbSet<WebhookClient> WebhookClients { get; set; }
        public DbSet<WebhookClientHeader> WebhookClientHeaders { get; set; }

        public MinimalWebhooksDbContext(DbContextOptions<MinimalWebhooksDbContext> options) : base(options)
        {}
    }
}