namespace Shared.Events;

public record TaskCreatedNotification(Guid TaskId, string? Title, DateTime DueDate);
