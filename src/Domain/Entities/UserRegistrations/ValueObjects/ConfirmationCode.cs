namespace Domain.Entities.UserRegistrations.ValueObjects;

public sealed record ConfirmationCode
{
    private const int ExpirationTime = 30;
    private ConfirmationCode(string codeHash, DateTime expiresAtUtc)
    {
        CodeHash = codeHash;
        ExpiresAtUtc = expiresAtUtc;
    }
    
    public string CodeHash { get; init; }

    public DateTime ExpiresAtUtc { get; init; }

    public static ConfirmationCode Create(out string code)
    {
        code = new Random().Next(100000, 999999).ToString();

        return new ConfirmationCode(
            BCrypt.Net.BCrypt.HashPassword(code), 
            DateTime.UtcNow.AddMinutes(ExpirationTime));
    }

    public bool Verify(string code)
    {
        return ExpiresAtUtc >= DateTime.UtcNow 
               && BCrypt.Net.BCrypt.Verify(code, CodeHash);
    }
}