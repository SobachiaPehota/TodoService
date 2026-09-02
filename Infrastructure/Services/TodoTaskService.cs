using Domain.DTOs;
using Domain.Interfaces.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class TodoTaskService : ITodoTaskService
{
    private readonly AppDbContext _context;

    public TodoTaskService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<TodoWithUserDto> Todos, int TotalCount)> GetTodosAsync(
        int limit,
        int offset,
        bool? completed = null,
        int? userId = null,
        string? todo = null,
        string sortBy = "id",
        string orderBy = "asc")
    {
        var query = _context.Todos
            .Include(t => t.User)
            .AsQueryable();

        if (completed.HasValue)
            query = query.Where(t => t.Completed == completed.Value);

        if (userId.HasValue)
            query = query.Where(t => t.UserId == userId.Value);

        if (!string.IsNullOrEmpty(todo))
            query = query.Where(u => u.TodoText.ToLower().Contains(todo.ToLower()));

        var totalCount = await query.CountAsync();

        query = sortBy.ToLower() switch
        {
            "userid" => orderBy.ToLower() == "desc"
                ? query.OrderByDescending(t => t.UserId)
                : query.OrderBy(t => t.UserId),
            "todo" => orderBy.ToLower() == "desc"
                ? query.OrderByDescending(t => t.TodoText)
                : query.OrderBy(t => t.TodoText),
            _ => orderBy.ToLower() == "desc"
                ? query.OrderByDescending(t => t.Id)
                : query.OrderBy(t => t.Id)
        };

        var todos = await query
            .Skip(offset)
            .Take(limit)
            .Select(t => new TodoWithUserDto
            {
                Id = t.Id,
                Todo = t.TodoText,
                Completed = t.Completed,
                User = new UserDto
                {
                    Id = t.User.Id,
                    FirstName = t.User.FirstName,
                    LastName = t.User.LastName,
                    Email = t.User.Email,
                    Username = t.User.Username
                }
            })
            .ToListAsync();

        return (todos, totalCount);
    }
}