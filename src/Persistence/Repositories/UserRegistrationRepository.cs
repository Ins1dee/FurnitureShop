using Domain.Entities.UserRegistrations;
using Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

internal sealed class UserRegistrationRepository 
    : Repository<UserRegistration, UserRegistrationId>, IUserRegistrationRepository
{
    public UserRegistrationRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default)
    {
        return !await DbContext.Users.AnyAsync(
            user => user.Email == Email.Create(email), cancellationToken);
    }
}