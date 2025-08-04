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


    [Fact]
    public void CannotCreateTask_WithEmptyTitle()
    {
        var ex = Assert.Throws<ArgumentException>(() => new TaskItem("", DateTime.UtcNow.AddDays(1)));
        Assert.Contains("Title cannot be empty", ex.Message);
    }

    [Fact]
    public void CannotCreateTask_WithPastDueDate()
    {
        var pastDate = DateTime.UtcNow.AddDays(-1);
        var ex = Assert.Throws<ArgumentException>(() => new TaskItem("Test", pastDate));
        Assert.Contains("Due date must be in the future", ex.Message);
    }

    [Fact]
    public void CompleteTask_SetsIsCompletedToTrue()
    {
        var task = new TaskItem("Test", DateTime.UtcNow.AddDays(1));
        task.Complete();
        Assert.True(task.IsCompleted);
    }

    [Fact]
    public void CompletingAnAlreadyCompletedTask_ThrowsInvalidOperationException()
    {
        var task = new TaskItem("Test", DateTime.UtcNow.AddDays(1));
        task.Complete();
        var ex = Record.Exception(() => task.Complete());
        Assert.IsType<InvalidOperationException>(ex);
    }

    [Fact]
public void CanUpdateTitle_WithValidValue()
{
    var task = new TaskItem("Original", DateTime.UtcNow.AddDays(1));
    task.UpdateTitle("Nuevo título");

    Assert.Equal("Nuevo título", task.Title);
}

[Fact]
public void CannotUpdateTitle_WithEmptyValue()
{
    var task = new TaskItem("Original", DateTime.UtcNow.AddDays(1));
    var ex = Assert.Throws<ArgumentException>(() => task.UpdateTitle(""));

    Assert.Contains("Title cannot be empty", ex.Message);
}

[Fact]
public void CanUpdateDueDate_WithValidFutureDate()
{
    var task = new TaskItem("Tarea", DateTime.UtcNow.AddDays(1));
    var newDate = DateTime.UtcNow.AddDays(5);

    task.UpdateDueDate(newDate);

    Assert.Equal(newDate, task.DueDate);
}

[Fact]
public void CannotUpdateDueDate_WithPastDate()
{
    var task = new TaskItem("Tarea", DateTime.UtcNow.AddDays(1));
    var pastDate = DateTime.UtcNow.AddDays(-2);

    var ex = Assert.Throws<ArgumentException>(() => task.UpdateDueDate(pastDate));

    Assert.Contains("Due date must be in the future", ex.Message);
}

}
