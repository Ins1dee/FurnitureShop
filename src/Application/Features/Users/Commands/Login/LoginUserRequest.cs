namespace Application.Features.Users.Commands.Login;

public record LoginUserRequest(
    string Email,
    string Password);