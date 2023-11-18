using Domain.Entities.Users;
using Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

internal sealed class UserRepository : Repository<User, UserId>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Users
            .FirstOrDefaultAsync(user => user!.Email == Email.Create(email), cancellationToken);
    }
}