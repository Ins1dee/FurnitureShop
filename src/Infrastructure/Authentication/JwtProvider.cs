using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Abstractions;
using Domain.Entities.Roles;
using Domain.Entities.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication;

public sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public string Generate(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email.Value)
        };
        
        foreach (Role userRole in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, userRole.Value));
        }
        
        SigningCredentials credentials = new(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_options.SecretKey)), 
            SecurityAlgorithms.HmacSha256);

        JwtSecurityToken securityToken = new(
            _options.Issuer,
            _options.Audience,
            claims,
            null,
            DateTime.Now.AddMinutes(_options.AccessTokenExpirationTime),
            credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}