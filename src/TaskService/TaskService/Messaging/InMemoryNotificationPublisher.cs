using System;
using Shared.Messaging;
using Shared.Events;

namespace TaskService.Messaging;


public class InMemoryNotificationPublisher : INotificationPublisher
{
    public static List<object> PublishedMessages { get; } = new();

    public Task PublishAsync<T>(T message)
    {
        PublishedMessages.Add(message!);
        Console.WriteLine($"📣 Notificación publicada: {message}");
        return Task.CompletedTask;
    }
}