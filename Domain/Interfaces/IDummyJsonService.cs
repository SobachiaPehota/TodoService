using Domain.Entities;

namespace Domain.Interfaces;

public interface IDummyJsonService
{
    Task<List<DummyUser>> GetUsersAsync();
    Task<List<DummyTodo>> GetTodosAsync();
}