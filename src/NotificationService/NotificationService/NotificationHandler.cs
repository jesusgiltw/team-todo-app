using System;
using Shared.Events;

namespace NotificationService;

public class NotificationHandler
{
    public Task Handle(TaskCreatedNotification notification)
    {
        Console.WriteLine($"📨 Notificación recibida en NotificationService: {notification.Title}");
        return Task.CompletedTask;
    }
}
