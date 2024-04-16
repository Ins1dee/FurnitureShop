using Ardalis.GuardClauses;
using Domain.Errors;

namespace Domain.Shared.ValueObjects;

public sealed record PasswordHash
{
    public const int MinLength = 5;
    public const int MaxLength = 50;
    public const string CorrectPasswordPattern = $@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).*$";
    private PasswordHash(string value)
    {
        Value = value;
    }
    
    public string Value { get; init; }

    public static PasswordHash Hash(string password)
    {
        Guard.Against.NullOrWhiteSpace(password);
        
        Guard.Against.OutOfRange(
            password.Length,
            nameof(password),
            MinLength,
            MaxLength,
            DomainErrors.PasswordHash.LengthOutOfRange().Message);
    
        //Guard.Against.InvalidFormat(
        //    password, 
        //    nameof(password), 
        //    CorrectPasswordPattern,
        //    DomainErrors.PasswordHash.InvalidFormat().Message);
        
        return new PasswordHash(BCrypt.Net.BCrypt.HashPassword(password));
    }
    
    public static PasswordHash Create(string hashedPassword)
    {
        return new PasswordHash(hashedPassword);
    }

    public bool Verify(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, Value);
    }
}