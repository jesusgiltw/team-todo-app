namespace Shared.Events;

public record TaskDueSoonNotification(Guid Id, string? Title, DateTime DueDate);
