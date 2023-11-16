namespace FurnitureShop.WebAPI.Requests;

public record RegisterUserRequest(
    string Firstname,
    string Lastname,
    string Email,
    string Password);