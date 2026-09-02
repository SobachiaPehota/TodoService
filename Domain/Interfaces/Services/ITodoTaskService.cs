using Domain.DTOs;

namespace Domain.Interfaces.Services;

public interface ITodoTaskService
{
    Task<(IEnumerable<TodoWithUserDto> Todos, int TotalCount)> GetTodosAsync(
        int limit,
        int offset,
        bool? completed = null,
        int? userId = null,
        string? todo = null,
        string sortBy = "id",
        string orderBy = "asc");
}