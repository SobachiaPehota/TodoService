using Domain.DTOs;
using Domain.Interfaces.Services;
using Domain.Utils;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<UserDto> Users, int TotalCount)> GetUsersAsync(
        int limit,
        int offset,
        string? firstName = null,
        string? lastName = null,
        string? email = null,
        string? username = null,
        string sortBy = "id",
        string orderBy = "asc")
    {
        var query = _context.Users.AsQueryable();

        if (!string.IsNullOrEmpty(firstName))
            query = query.Where(u => u.FirstName.ToLower().Contains(firstName.ToLower()));

        if (!string.IsNullOrEmpty(lastName))
            query = query.Where(u => u.LastName.ToLower().Contains(lastName.ToLower()));

        if (!string.IsNullOrEmpty(email))
            query = query.Where(u => u.Email.ToLower().Contains(email.ToLower()));

        if (!string.IsNullOrEmpty(username))
            query = query.Where(u => u.Username.ToLower().Contains(username.ToLower()));

        var totalCount = await query.CountAsync();

        query = sortBy.ToLower() switch
        {
            "lastname" => orderBy.ToLower() == "desc"
                ? query.OrderByDescending(u => u.LastName)
                : query.OrderBy(u => u.LastName),
            "firstname" => orderBy.ToLower() == "desc"
                ? query.OrderByDescending(u => u.FirstName)
                : query.OrderBy(u => u.FirstName),
            "username" => orderBy.ToLower() == "desc"
                ? query.OrderByDescending(u => u.Username)
                : query.OrderBy(u => u.Username),
            _ => orderBy.ToLower() == "desc"
                ? query.OrderByDescending(u => u.Id)
                : query.OrderBy(u => u.Id)
        };

        var users = await query
            .Skip(offset)
            .Take(limit)
            .Select(u => MapExtension.FromEntity(u))
            .ToListAsync();

        return (users, totalCount);
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        return user != null ? MapExtension.FromEntity(user) : null;
    }

    public async Task<IEnumerable<TodoDto>> GetUserTodosAsync(
        int userId,
        int limit,
        int offset,
        bool? completed = null,
        string? todo = null,
        string sortBy = "id",
        string orderBy = "asc")
    {
        var query = _context.Todos
            .Where(t => t.UserId == userId)
            .AsQueryable();

        if (completed.HasValue)
            query = query.Where(t => t.Completed == completed.Value);

        if (!string.IsNullOrEmpty(todo))
            query = query.Where(u => u.TodoText.ToLower().Contains(todo.ToLower()));

        query = sortBy.ToLower() switch
        {
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
            .Select(t => MapExtension.FromEntity(t))
            .ToListAsync();

        return todos;
    }
}