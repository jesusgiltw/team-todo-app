using System;

namespace TaskService.Domain;

public class TaskItem
{
    public string Title { get; }
    public DateTime DueDate { get; }
    public bool IsCompleted { get; private set; }

    public TaskItem(string title, DateTime dueDate)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));
        if (dueDate < DateTime.UtcNow)
            throw new ArgumentException("Due date must be in the future", nameof(dueDate));

        Title = title;
        DueDate = dueDate;
        IsCompleted = false;
    }

    public void Complete()
    {
        IsCompleted = true;
    }
}

