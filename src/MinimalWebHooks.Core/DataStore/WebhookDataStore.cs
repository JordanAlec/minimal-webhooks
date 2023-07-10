using Microsoft.EntityFrameworkCore;
using MinimalWebHooks.Core.Enum;
using MinimalWebHooks.Core.Extensions;
using MinimalWebHooks.Core.Interfaces;
using MinimalWebHooks.Core.Models.DbSets;

namespace MinimalWebHooks.Core.DataStore;

public class WebhookDataStore : IWebhookDataStore
{
    private readonly MinimalWebhooksDbContext _context;

    public WebhookDataStore(MinimalWebhooksDbContext context) => _context = context;

    public async Task<WebhookClient?> GetById(int id, bool skipDisabledClients = true)
    {
        var clientsQueryable = _context.WebhookClients.AsQueryable().Where(x => x.Id == id);
        if (skipDisabledClients) clientsQueryable = clientsQueryable.Where(x => !x.Disabled).AsQueryable();
        return await clientsQueryable.Include(x => x.ClientHeaders).FirstOrDefaultAsync();
    }

    public async Task<WebhookClient?> GetByName(string name) =>
        await _context.WebhookClients.Include(x => x.ClientHeaders).FirstOrDefaultAsync(w => w.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) && !w.Disabled);

    public async Task<List<WebhookClient>?> Get() =>
        await _context.WebhookClients.Where(w => !w.Disabled)
            .Include(x => x.ClientHeaders)
            .ToListAsync();

    public async Task<List<WebhookClient>?> GetByEntity<T>(T data, WebhookActionType actionType) =>
        await _context.WebhookClients
            .Where(x => x.EntityTypeName.Equals(data.GetEntityTypeName(), StringComparison.InvariantCultureIgnoreCase))
            .Where(x => x.ActionType == actionType)
            .Where(x => !x.Disabled)
            .Include(x => x.ClientHeaders)
            .ToListAsync();

    public async Task<WebhookClient?> Create(WebhookClient client)
    {
        _context.WebhookClients.Add(client);
        var result = await _context.SaveChangesAsync();
        return result > 0 ? client : null;
    }

    public async Task<bool> Update(WebhookClient client)
    {
        _context.WebhookClients.Update(client);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> Delete(List<WebhookClientHeader> headers)
    {
        _context.RemoveRange(headers);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }
}