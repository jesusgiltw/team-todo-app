using Shared.Messaging;

public class InMemoryNotificationPublisher : INotificationPublisher
{
    private readonly List<Func<object, Task>> _handlers = new();

    public void Subscribe<T>(Func<T, Task> handler)
    {
        _handlers.Add(async (msg) => await handler((T)msg));
    }

    public async Task PublishAsync<T>(T notification)
    {
        Console.WriteLine($"📣 Publicando evento: {notification}");

        foreach (var handler in _handlers)
        {
            await handler(notification!);
        }
    }
}
