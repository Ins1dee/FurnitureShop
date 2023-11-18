namespace Infrastructure.Services.Email;

public sealed class EmailOptions
{
    public string From { get; set; } = string.Empty;

    public string SmtpServer { get; set; } = string.Empty;

    public int Port { get; set; }

    public string Password { get; set; } = string.Empty;
}