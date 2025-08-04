using TaskService.Domain;
using TaskService.Infrastructure;

namespace NotificationService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ITaskRepository _taskRepository;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
        _taskRepository = new InMemoryTaskRepository();
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {   
        Console.WriteLine($"Worker running at: {DateTimeOffset.Now}");
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            var next24h = now.AddHours(24);

            var tasksAboutToExpire = await _taskRepository.GetTasksDueBetweenAsync(now, next24h, stoppingToken);

            foreach (var task in tasksAboutToExpire)
            {
                _logger.LogInformation("Tarea próxima a caducar: {Title} - Vence: {DueDate}", task.Title, task.DueDate);
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}
