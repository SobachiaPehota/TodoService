using Domain.DTOs;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/todos")]
public class TodosController : ControllerBase
{
    private readonly ITodoTaskService _todoService;

    public TodosController(ITodoTaskService todoService)
    {
        _todoService = todoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoWithUserDto>>> GetTodos(
        [FromQuery] int limit = 10,
        [FromQuery] int offset = 0,
        [FromQuery] bool? completed = null,
        [FromQuery] int? userId = null,
        [FromQuery] string? todo = null,
        [FromQuery] string sortBy = "id",
        [FromQuery] string orderBy = "asc")
    {
        var (todos, totalCount) = await _todoService.GetTodosAsync(
            limit, offset, completed, userId, todo, sortBy, orderBy);

        Response.Headers.Append("X-Total-Count", totalCount.ToString());
        return Ok(todos);
    }
}