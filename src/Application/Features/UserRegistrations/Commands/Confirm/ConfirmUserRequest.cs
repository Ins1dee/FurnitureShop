namespace Application.Features.UserRegistrations.Commands.Confirm;

public record ConfirmUserRequest(
    Guid UserRegistrationId,
    string ConfirmationCode);