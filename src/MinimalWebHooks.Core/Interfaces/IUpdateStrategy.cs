using MinimalWebHooks.Core.Models;

namespace MinimalWebHooks.Core.Interfaces;

public interface IUpdateStrategy
{
    public bool ShouldUpdate();
    public Task<UpdateResult> Update();
}