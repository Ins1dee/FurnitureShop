namespace Domain.Shared;

public class Result : IResult
{
    protected Result(bool isSuccess, Error error, ResultStatus status)
    {
        if (isSuccess && !error.Equals(Error.None) && !status.Equals(ResultStatus.Ok))
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error.Equals(Error.None) && status.Equals(ResultStatus.Ok))
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
        Status = status;
    }

    public bool IsSuccess { get; }
    
    public bool IsFailure => !IsSuccess;
    
    public Error Error { get; }
    
    public ResultStatus Status { get; }
    
    public static Result Success() => new(true, Error.None, ResultStatus.Ok);
    
    public static Result BadRequest(Error error) => new(false, error, ResultStatus.BadRequest);
    
    public static Result Forbidden(Error error) => new(false, error, ResultStatus.Forbidden);
    
    public static Result NotFound(Error error) => new(false, error, ResultStatus.NotFound);
    
    public static Result Unauthorized(Error error) => new(false, error, ResultStatus.Unauthorized);
    
    protected static Result<TValue> Success<TValue>(TValue value) 
        => new(value, true, Error.None, ResultStatus.Ok);

    public static Result<TValue> BadRequest<TValue>(Error error) 
        => new(default, false, error, ResultStatus.BadRequest);
    
    public static Result<TValue> Forbidden<TValue>(Error error) 
        => new(default, false, error, ResultStatus.Forbidden);
    
    public static Result<TValue> NotFound<TValue>(Error error) 
        => new(default, false, error, ResultStatus.NotFound);
    
    public static Result<TValue> Unauthorized<TValue>(Error error) 
        => new(default, false, error, ResultStatus.Unauthorized);

    public static IResult IdempotencyResult() => new Result(true, Error.None, ResultStatus.Ok);
}

public class Result<TValue> : Result, IResult
{
    private readonly TValue? _value;
    
    protected internal Result(
        TValue? value, 
        bool isSuccess, 
        Error error, 
        ResultStatus status)
        : base(isSuccess, error, status)
    {
        _value = value;
    }

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException();
    
    public new static IResult IdempotencyResult() 
        => new Result<TValue>(default, true, Error.None, ResultStatus.Ok);
    
    public static implicit operator Result<TValue>(TValue value)
    {
        return new Result<TValue>(value, true, Error.None, ResultStatus.Ok);
    }
}

public enum ResultStatus
{
    Ok = 200,
    BadRequest = 400,
    Forbidden = 403,
    NotFound = 404,
    Unauthorized = 401
}