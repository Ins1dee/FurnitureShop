using Application.Abstractions;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Infrastructure.Services.Email;

public class EmailService : IEmailService
{
    private readonly EmailOptions _emailOptions;

    public EmailService(IOptions<EmailOptions> emailOptions)
    {
        _emailOptions = emailOptions.Value;
    }

    public async Task SendEmailAsync(
        string to, 
        string subject, 
        string body, 
        CancellationToken cancellationToken)
    {
        MimeMessage message = new();
        message.From.Add(MailboxAddress.Parse(_emailOptions.From));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart(TextFormat.Html) { Text = body };

        using SmtpClient client = new();
        await client.ConnectAsync(
            _emailOptions.SmtpServer, 
            _emailOptions.Port, 
            SecureSocketOptions.StartTls, 
            cancellationToken);
        
        await client.AuthenticateAsync(
            _emailOptions.From, 
            _emailOptions.Password, 
            cancellationToken);
        
        await client.SendAsync(message, cancellationToken);
    }
}