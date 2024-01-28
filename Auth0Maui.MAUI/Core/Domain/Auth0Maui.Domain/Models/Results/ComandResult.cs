

namespace Auth0Maui.Domain.Models.Results;

public class CommandResult<T>
{
    // Private constructor ensures control over the creation of instances
    private CommandResult(bool isSuccessful, string message, T data)
    {
        IsSuccessful = isSuccessful;
        Message = message;
        Data = data;
    }

    public bool IsSuccessful { get; }
    public string Message { get; }
    public T Data { get; }

    // Factory method for success
    public static CommandResult<T> Success(T data, string message = "")
    {
        return new CommandResult<T>(true, message, data);
    }

    // Factory method for failure
    public static CommandResult<T> Failure(string message)
    {
        return new CommandResult<T>(false, message, default);
    }
}