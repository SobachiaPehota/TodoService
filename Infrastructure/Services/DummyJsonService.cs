using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace Infrastructure.Services;

public class DummyJsonService : IDummyJsonService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<DummyJsonService> _logger;

    public DummyJsonService(HttpClient httpClient, ILogger<DummyJsonService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<DummyUser>> GetUsersAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("users");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<DummyUsersResponse>();
            return result?.Users ?? new List<DummyUser>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching users from DummyJSON");
            return new List<DummyUser>();
        }
    }

    public async Task<List<DummyTodo>> GetTodosAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("todos?limit=150");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<DummyTodosResponse>();
            return result?.Todos ?? new List<DummyTodo>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching todos from DummyJSON");
            return new List<DummyTodo>();
        }
    }

    private class DummyUsersResponse
    {
        public List<DummyUser> Users { get; set; } = new();
        public int Total { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
    }

    private class DummyTodosResponse
    {
        public List<DummyTodo> Todos { get; set; } = new();
        public int Total { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
    }
}