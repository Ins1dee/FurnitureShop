namespace Domain.Entities.UserRegistrations;

public interface IUserRegistrationRepository
{
    Task AddAsync(UserRegistration userRegistration, CancellationToken cancellationToken = default);

    Task<UserRegistration?> GetByIdAsync(UserRegistrationId id, CancellationToken cancellationToken = default);
    Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default);
}