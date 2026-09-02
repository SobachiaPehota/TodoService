using Domain.DTOs;

namespace Domain.Interfaces.Services;

public interface IUserService
{
    Task<(IEnumerable<UserDto> Users, int TotalCount)> GetUsersAsync(
        int limit,
        int offset,
        string? firstName = null,
        string? lastName = null,
        string? email = null,
        string? username = null,
        string sortBy = "id",
        string orderBy = "asc");

    Task<UserDto?> GetUserByIdAsync(int id);
    Task<IEnumerable<TodoDto>> GetUserTodosAsync(
        int userId,
        int limit,
        int offset,
        bool? completed = null,
        string? todo = null,
        string sortBy = "id",
        string orderBy = "asc");
}