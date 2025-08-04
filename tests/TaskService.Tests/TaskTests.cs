using TaskService.Domain;

namespace TaskService.Tests;

public class TaskTests
{
    [Fact]
    public void CanCreateTask_WithValidData()
    {
        var task = new TaskItem("Learn TDD", DateTime.UtcNow.AddDays(3));

        Assert.Equal("Learn TDD", task.Title);
        Assert.False(task.IsCompleted);
    }
}
