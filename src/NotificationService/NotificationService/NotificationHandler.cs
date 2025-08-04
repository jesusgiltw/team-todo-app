using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shared.Events;

namespace NotificationService;

public interface INotificationHandler
{
    Task Handle(TaskCreatedNotification notification);
}

public class NotificationHandler : INotificationHandler
{
    private readonly ILogger<NotificationHandler> _logger;

    public NotificationHandler(ILogger<NotificationHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(TaskCreatedNotification notification)
    {
        // Aquí puedes agregar lógica adicional, como enviar emails, guardar en base de datos, etc.
        _logger.LogInformation("📨 Notificación recibida: {Title} con fecha {DueDate}", notification.Title, notification.DueDate);
        return Task.CompletedTask;
    }
}