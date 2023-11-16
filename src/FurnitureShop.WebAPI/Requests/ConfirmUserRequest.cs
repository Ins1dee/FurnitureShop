namespace FurnitureShop.WebAPI.Requests;

public record ConfirmUserRequest(
    Guid UserRegistrationId,
    string ConfirmationCode);