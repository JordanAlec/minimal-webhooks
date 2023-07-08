using MinimalWebHooks.Core.Validation;

namespace MinimalWebHooks.Core.Interfaces;

public interface IValidationRule
{
    Task<ValidationResult> Validate();
}