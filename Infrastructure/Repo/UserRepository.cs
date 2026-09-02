using Domain.Entities;
using Domain.Interfaces.Repos;
using Infrastructure.Data;

namespace TodoService.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task AddOrUpdateAsync(User user)
    {
        var existing = await _context.Users.FindAsync(user.Id);
        if (existing == null)
        {
            await _context.Users.AddAsync(user);
        }
        else
        {
            _context.Entry(existing).CurrentValues.SetValues(user);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}