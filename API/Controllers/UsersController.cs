using Domain.DTOs;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers(
        [FromQuery] int limit = 10,
        [FromQuery] int offset = 0,
        [FromQuery] string? firstName = null,
        [FromQuery] string? lastName = null,
        [FromQuery] string? email = null,
        [FromQuery] string? username = null,
        [FromQuery] string sortBy = "id",
        [FromQuery] string orderBy = "asc")
    {
        var (users, totalCount) = await _userService.GetUsersAsync(
            limit, offset, firstName, lastName, email, username, sortBy, orderBy);

        Response.Headers.Append("X-Total-Count", totalCount.ToString());
        return Ok(users);
    }

    [HttpGet("{id:int}/todos")]
    public async Task<ActionResult<IEnumerable<TodoDto>>> GetUserTodos(
        int id,
        [FromQuery] int limit = 10,
        [FromQuery] int offset = 0,
        [FromQuery] bool? completed = null,
        [FromQuery] string? todo = null,
        [FromQuery] string sortBy = "id",
        [FromQuery] string orderBy = "asc")
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
            return NotFound($"User with id {id} not found");

        var todos = await _userService.GetUserTodosAsync(
            id, limit, offset, completed, todo, sortBy, orderBy);

        return Ok(todos);
    }
}