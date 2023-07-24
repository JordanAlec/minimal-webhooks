using MinimalWebHooks.Core.Interfaces;
using MinimalWebHooks.Core.Models;

namespace MinimalWebHooks.Core.Strategies;

public class UpdateWebhookProcessor
{
    private readonly List<IUpdateStrategy> _updates;
    private readonly List<UpdateResult> _updateResults;
    public UpdateWebhookProcessor(List<IUpdateStrategy> updates)
    {
        _updates = updates;
        _updateResults = new List<UpdateResult>();
    }

    public async Task Update()
    {
        foreach (var updateStrategy in _updates.Where(x => x.ShouldUpdate()))
            _updateResults.Add(await updateStrategy.Update());
    }

    public bool HasFailures() => _updateResults.Any(x => !x.Success);
    public string GetMessage() => string.Join(", ", _updateResults.Where(x => !string.IsNullOrWhiteSpace(x.Message)).Select(x => x.Message));
}