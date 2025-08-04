using System;

namespace TaskService.Domain;

public class TaskItem
{
    public string? Title { get; private set; }
    public DateTime DueDate { get; private set; }
    public bool IsCompleted { get; private set; }

    public TaskItem(string? title, DateTime dueDate)
    {
        SetTitle(title);
        SetDueDate(dueDate);
        IsCompleted = false;
    }

    private void SetTitle(string? title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));
        Title = title;
    }

    private void SetDueDate(DateTime dueDate)
    {
        if (dueDate <= DateTime.UtcNow)
            throw new ArgumentException("Due date must be in the future", nameof(dueDate));
        DueDate = dueDate;
    }

    public void Complete()
    {
        if (IsCompleted)
            throw new InvalidOperationException("Task is already completed.");
        IsCompleted = true;
    }

    public void UpdateTitle(string newTitle)
    {
        SetTitle(newTitle);
    }

    public void UpdateDueDate(DateTime newDueDate)
    {
        SetDueDate(newDueDate);
    }
}


