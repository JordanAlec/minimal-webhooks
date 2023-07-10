using Microsoft.EntityFrameworkCore;
using MinimalWebHooks.Core.Enum;
using MinimalWebHooks.Core.Extensions;
using MinimalWebHooks.Core.Interfaces;
using MinimalWebHooks.Core.Models;

namespace MinimalWebHooks.Core.DataStore;

public class WebhookDataStore : IWebhookDataStore
{
    private readonly MinimalWebhooksDbContext _context;

    public WebhookDataStore(MinimalWebhooksDbContext context) => _context = context;

    public async Task<WebhookClient?> GetById(int id) =>
        await _context.WebhookClients.FirstOrDefaultAsync(w => w.Id == id && !w.Disabled);
    public async Task<WebhookClient?> GetByName(string name) =>
        await _context.WebhookClients.FirstOrDefaultAsync(w => w.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) && !w.Disabled);

    public async Task<List<WebhookClient>?> Get() =>
        await _context.WebhookClients.Where(w => !w.Disabled)
            .ToListAsync();

    public async Task<List<WebhookClient>?> GetByEntity<T>(T data, WebhookActionType actionType) =>
        await _context.WebhookClients
            .Where(w => w.EntityTypeName.Equals(data.GetEntityTypeName(), StringComparison.InvariantCultureIgnoreCase))
            .Where(w => w.ActionType == actionType)
            .Where(w => !w.Disabled)
            .ToListAsync();

    public async Task<WebhookClient?> Create(WebhookClient client)
    {
        _context.WebhookClients.Add(client);
        var result = await _context.SaveChangesAsync();
        return result > 0 ? client : null;
    }

    public async Task<bool> Disable(WebhookClient client)
    {
        client.Disabled = true;
        _context.WebhookClients.Update(client);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }


}