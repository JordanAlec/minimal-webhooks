namespace MinimalWebHooks.Core.Validation;

public class ValidationResult
{
    public bool IsValid { get; private set; }
    public string Message { get; private set; }

    private ValidationResult Result(bool isValid, string message)
    {
        IsValid = isValid;
        Message = message;
        return this;
    }

    public ValidationResult Success(string message) => Result(true, message);
    public ValidationResult Failure(string message) => Result(false, message);

}