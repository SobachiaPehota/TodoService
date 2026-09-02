using Domain.DTOs;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using Xunit;

namespace TodoServiceTest;

public class ApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public ApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase("TestDb"));
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                if (!db.Users.Any())
                {
                    db.Database.EnsureDeletedAsync();
                    db.Database.EnsureCreatedAsync();
                    db.Users.AddRange(TestData.GetUsers());
                    db.Todos.AddRange(TestData.GetTodos());
                    db.SaveChanges();
                }
            });
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetUsers_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/users");

        response.EnsureSuccessStatusCode();
        var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();
        Assert.NotNull(users);
        Assert.Equal(3, users.Count);
    }

    [Fact]
    public async Task GetUsers_WithLimit_ReturnsLimitedUsers()
    {
        var response = await _client.GetAsync("/api/users?limit=1&offset=0");

        response.EnsureSuccessStatusCode();
        var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();
        Assert.Single(users!);
    }

    [Fact]
    public async Task GetUsers_WithFilter_ReturnsFilteredUsers()
    {
        var response = await _client.GetAsync("/api/users?firstName=Иван");

        response.EnsureSuccessStatusCode();
        var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();
        Assert.Single(users!);
        Assert.Equal("Иван", users![0].FirstName);
    }

    [Fact]
    public async Task GetUsers_WithSorting_ReturnsSortedUsers()
    {
        var response = await _client.GetAsync("/api/users?sortBy=lastName&orderBy=desc");

        response.EnsureSuccessStatusCode();
        var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();
        Assert.Equal("Сидоров", users![0].LastName);
    }

    [Fact]
    public async Task GetUserTodos_WithValidUserId_ReturnsTodos()
    {
        var response = await _client.GetAsync("/api/users/1/todos");

        response.EnsureSuccessStatusCode();
        var todos = await response.Content.ReadFromJsonAsync<List<TodoDto>>();
        Assert.Equal(2, todos!.Count);
    }

    [Fact]
    public async Task GetUserTodos_WithInvalidUserId_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/users/999/todos");

        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetTodos_ReturnsAllTodos()
    {
        var response = await _client.GetAsync("/api/todos");

        response.EnsureSuccessStatusCode();
        var todos = await response.Content.ReadFromJsonAsync<List<TodoWithUserDto>>();
        Assert.Equal(5, todos!.Count);
        Assert.All(todos, t => Assert.NotNull(t.User));
    }

    [Fact]
    public async Task GetTodos_WithCompletedFilter_ReturnsCompletedTodos()
    {
        var response = await _client.GetAsync("/api/todos?completed=true");

        response.EnsureSuccessStatusCode();
        var todos = await response.Content.ReadFromJsonAsync<List<TodoWithUserDto>>();
        Assert.Equal(2, todos!.Count);
        Assert.All(todos, t => Assert.True(t.Completed));
    }

    [Fact]
    public async Task GetTodos_WithUserIdFilter_ReturnsUserTodos()
    {
        var response = await _client.GetAsync("/api/todos?userId=1");

        response.EnsureSuccessStatusCode();
        var todos = await response.Content.ReadFromJsonAsync<List<TodoWithUserDto>>();
        Assert.Equal(2, todos!.Count);
        Assert.All(todos, t => Assert.Equal(1, t.User.Id));
    }
}