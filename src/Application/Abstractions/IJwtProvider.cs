using Domain.Entities.Users;

namespace Application.Abstractions;

public interface IJwtProvider
{
    string Generate(User user);
}