using Infrastructure.Services.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Infrastructure.OptionSetup;

public class EmailOptionsSetup : IConfigureOptions<EmailOptions>
{
    private const string SectionName = "EmailOptions";

    private readonly IConfiguration _configuration;

    public EmailOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(EmailOptions options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}