namespace FurnitureShop.WebAPI.Requests;

public record LoginUserRequest(
    string Email,
    string Password);