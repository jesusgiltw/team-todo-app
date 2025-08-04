using TaskService.Domain;
using TaskService.Infrastructure;

public class InMemoryTaskRepositoryTests
{
    [Fact]
    public async Task AddAndRetrieveTask_WorksCorrectly()
    {
        var repo = new InMemoryTaskRepository();
        var task = new TaskItem("Test task", DateTime.UtcNow.AddDays(1));

        await repo.AddAsync(task);
        var retrieved = await repo.GetByIdAsync(task.Id);

        Assert.NotNull(retrieved);
        Assert.Equal(task.Title, retrieved!.Title);
    }

    [Fact]
    public async Task GetAll_ReturnsAllTasks()
    {
        var repo = new InMemoryTaskRepository();

        var task1 = new TaskItem("Task 1", DateTime.UtcNow.AddDays(1));
        var task2 = new TaskItem("Task 2", DateTime.UtcNow.AddDays(2));

        await repo.AddAsync(task1);
        await repo.AddAsync(task2);

        var all = await repo.GetAllAsync();

        Assert.Contains(all, t => t.Title == "Task 1");
        Assert.Contains(all, t => t.Title == "Task 2");
    }
}
