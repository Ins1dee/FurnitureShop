namespace FurnitureShop.Domain.Shared;

public class Result : IResult
{
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && !error.Equals(Error.None))
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error.Equals(Error.None))
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    
    public bool IsFailure => !IsSuccess;
    
    public Error Error { get; }
    
    public static Result Success() => new(true, Error.None);
    
    public static Result Failure(Error error) => new(false, error);
    
    protected static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

    public static IResult IdempotencyResult() => new Result(true, Error.None);
}

public class Result<TValue> : IResult
{
    private readonly TValue? _value;
    
    public bool IsSuccess { get; }
    
    public bool IsFailure => !IsSuccess;
    
    public Error Error { get; }
    
    protected internal Result(TValue? value, bool isSuccess, Error error)
    {
        _value = value;
        IsSuccess = isSuccess;
        Error = error;
    }

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException();
    
    public static IResult IdempotencyResult() => new Result<TValue>(default, true, Error.None);
    
    public static implicit operator Result<TValue>(TValue value)
    {
        return new Result<TValue>(value, true, Error.None);
    }
}