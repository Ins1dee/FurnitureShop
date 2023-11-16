using FurnitureShop.Domain.Entities.Users;

namespace FurnitureShop.Application.Abstractions;

public interface IJwtProvider
{
    string Generate(User user);
}