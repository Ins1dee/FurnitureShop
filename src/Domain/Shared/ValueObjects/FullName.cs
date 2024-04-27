using Ardalis.GuardClauses;
using Domain.Errors;

namespace Domain.Shared.ValueObjects;

public sealed record FullName
{
    public const int MinLength = 2;
    public const int MaxLength = 20;

    private FullName(string firstname, string lastname)
    {
        Firstname = firstname;
        Lastname = lastname;
    }
    
    public string Firstname { get; init; }
    
    public string Lastname { get; init; }
    
    public static FullName Create(string firstname, string lastname)
    {
        Guard.Against.NullOrWhiteSpace(firstname);
        Guard.Against.NullOrWhiteSpace(lastname);
        
        Guard.Against.OutOfRange(
            firstname.Length, 
            nameof(firstname), 
            MinLength, 
            MaxLength,
            DomainErrors.FullName.FirstNameLengthOutOfRange().Message);
        
        Guard.Against.OutOfRange(
            lastname.Length, 
            nameof(firstname), 
            MinLength, 
            MaxLength,
            DomainErrors.FullName.LastnameLengthOutOfRange().Message);

        return new FullName(firstname, lastname);
    }

    public override string ToString()
    {
        return $"{Firstname} {Lastname}";
    }
}