using Domain.Entities;

namespace Domain.Interfaces.Repos
{
    public interface ITodoRepository
    {
        Task AddOrUpdateAsync(Todo todo);
        Task SaveChangesAsync();
    }
}