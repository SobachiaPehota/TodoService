using Domain.Entities;

namespace Domain.Interfaces.Repos;

public interface IUserRepository
{
    Task AddOrUpdateAsync(User user);
    Task SaveChangesAsync();
}