using Domain.DTOs;
using Domain.Entities;

namespace Domain.Utils
{
    public static class MapExtension
    {

        public static TodoWithUserDto TodoFromEntity(Todo todo)
        {
            return new TodoWithUserDto
            {
                Id = todo.Id,
                Todo = todo.TodoText,
                Completed = todo.Completed,
                User = FromEntity(todo.User)
            };
        }

        public static UserDto FromEntity(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Username
            };
        }

        public static User ToEntity(UserDto dto)
        {
            return new User
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Username = dto.Username,
            };
        }

        public static TodoDto FromEntity(Todo todo)
        {
            return new TodoDto
            {
                Id = todo.Id,
                Todo = todo.TodoText,
                Completed = todo.Completed
            };
        }

        public static Todo ToEntity(TodoDto dto)
        {
            return new Todo
            {
                Id = dto.Id,
                TodoText = dto.Todo,
                Completed = dto.Completed,
            };
        }
    }
}
