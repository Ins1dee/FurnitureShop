namespace Application.Features.UserRegistrations.Commands.Create;

public record RegisterUserRequest(
    string Firstname,
    string Lastname,
    string Email,
    string Password,
    string Role);