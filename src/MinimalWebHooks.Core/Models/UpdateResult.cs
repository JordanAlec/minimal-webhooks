using System.Reflection.Metadata.Ecma335;

namespace MinimalWebHooks.Core.Models;

public class UpdateResult
{
    public bool Success { get; private set; }
    public string? Message { get; private set; }

    private UpdateResult CreateResult(bool success, string? message = null)
    {
        Success = success;
        Message = message;
        return this;
    }

    public UpdateResult ResultSuccess() => CreateResult(true);
    public UpdateResult ResultFailure(string failureReason) => CreateResult(false, failureReason);
}