using Domain.Entities;
using Domain.Interfaces.Repos;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repo;

public class TodoRepository : ITodoRepository
{
    private readonly AppDbContext _context;

    public TodoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddOrUpdateAsync(Todo todo)
    {
        var existing = await _context.Todos.FindAsync(todo.Id);
        if (existing == null)
        {
            await _context.Todos.AddAsync(todo);
        }
        else
        {
            _context.Entry(existing).CurrentValues.SetValues(todo);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}