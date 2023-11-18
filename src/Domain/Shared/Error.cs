namespace Domain.Shared;

public class Error
{
    public static readonly Error None = new(string.Empty);
    
    public Error(string message)
    {
        Message = message;
    }

    public string Message { get; }
}