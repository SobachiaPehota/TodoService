using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class DataSyncService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<DataSyncService> _logger;

    public DataSyncService(
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration,
        ILogger<DataSyncService> logger)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Data Sync Service started");

        await SyncDataAsync();

        var intervalMinutes = _configuration.GetValue<int>("SyncSettings:IntervalMinutes", 5);
        var interval = TimeSpan.FromMinutes(intervalMinutes);

        _logger.LogInformation($"Sync interval set to {intervalMinutes} minutes");

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(interval, stoppingToken);

            try
            {
                await SyncDataAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during data synchronization");
            }
        }
    }

    private async Task SyncDataAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var todoRepository = scope.ServiceProvider.GetRequiredService<ITodoRepository>();
        var dummyJsonService = scope.ServiceProvider.GetRequiredService<IDummyJsonService>();

        _logger.LogInformation("Starting data synchronization");

        var dummyUsers = await dummyJsonService.GetUsersAsync();
        _logger.LogInformation($"Received {dummyUsers.Count} users from DummyJSON");

        foreach (var dummyUser in dummyUsers)
        {
            var user = new User
            {
                Id = dummyUser.Id,
                FirstName = dummyUser.FirstName,
                LastName = dummyUser.LastName,
                Email = dummyUser.Email,
                Username = dummyUser.Username
            };

            await userRepository.AddOrUpdateAsync(user);
        }

        await userRepository.SaveChangesAsync();
        _logger.LogInformation("Users synchronized successfully");

        var dummyTodos = await dummyJsonService.GetTodosAsync();
        _logger.LogInformation($"Received {dummyTodos.Count} todos from DummyJSON");
        var todosForCurrentUsers = dummyTodos.Where(x => dummyUsers.Select(x => x.Id).Contains(x.UserId));

        foreach (var dummyTodo in todosForCurrentUsers)
        {
            var todo = new Todo
            {
                Id = dummyTodo.Id,
                TodoText = dummyTodo.Todo,
                Completed = dummyTodo.Completed,
                UserId = dummyTodo.UserId
            };

            await todoRepository.AddOrUpdateAsync(todo);
        }

        await todoRepository.SaveChangesAsync();
        _logger.LogInformation("Todos synchronized successfully");
    }
}