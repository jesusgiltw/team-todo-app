namespace Shared.Messaging;

public interface INotificationPublisher
{
    Task PublishAsync<T>(T message);
}