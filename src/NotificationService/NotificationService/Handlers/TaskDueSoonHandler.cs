using Shared.Events;

namespace NotificationService.Handlers;

public class TaskDueSoonHandler
{
    public Task Handle(TaskDueSoonNotification notification)
    {
        Console.WriteLine($"🔔 Tarea próxima a vencer: {notification.Title} (vence: {notification.DueDate})");
        return Task.CompletedTask;
    }
}
