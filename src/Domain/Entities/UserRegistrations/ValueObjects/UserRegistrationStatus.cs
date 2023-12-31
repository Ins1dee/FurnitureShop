namespace Domain.Entities.UserRegistrations.ValueObjects;

public sealed record UserRegistrationStatus
{
    private UserRegistrationStatus(string value)
    {
        Value = value;
    }
    
    public static UserRegistrationStatus WaitingForConfirmation =>
        new(nameof(WaitingForConfirmation));

    public static UserRegistrationStatus Confirmed => new(nameof(Confirmed));

    public static UserRegistrationStatus Expired => new(nameof(Expired));

    public string Value { get; }
}