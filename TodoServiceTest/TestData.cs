using Domain.Entities;

namespace TodoServiceTest;

public static class TestData
{
    public static List<User> GetUsers()
    {
        return new List<User>
        {
            new User { Id = 1, FirstName = "Иван", LastName = "Иванов", Email = "ivan@test.com", Username = "ivanov" },
            new User { Id = 2, FirstName = "Петр", LastName = "Петров", Email = "petr@test.com", Username = "petrov" },
            new User { Id = 3, FirstName = "Сидор", LastName = "Сидоров", Email = "sidor@test.com", Username = "sidorov" }
        };
    }

    public static List<Todo> GetTodos()
    {
        return new List<Todo>
        {
            new Todo { Id = 1, TodoText = "Купить продукты", Completed = false, UserId = 1 },
            new Todo { Id = 2, TodoText = "Помыть посуду", Completed = true, UserId = 1 },
            new Todo { Id = 3, TodoText = "Сделать домашку", Completed = false, UserId = 2 },
            new Todo { Id = 4, TodoText = "Погулять с собакой", Completed = true, UserId = 2 },
            new Todo { Id = 5, TodoText = "Прочитать книгу", Completed = false, UserId = 3 }
        };
    }
}