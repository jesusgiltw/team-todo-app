using System.Collections.Concurrent;

namespace TaskService.Infrastructure;
using TaskService.Domain;

public class InMemoryTaskRepository : ITaskRepository
{
    private readonly ConcurrentDictionary<Guid, TaskItem> _storage = new();

    public Task AddAsync(TaskItem task)
    {
        _storage[task.Id] = task;
        return Task.CompletedTask;
    }

    public Task<TaskItem?> GetByIdAsync(Guid id)
    {
        _storage.TryGetValue(id, out var task);
        return Task.FromResult(task);
    }

    public Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        return Task.FromResult(_storage.Values.AsEnumerable());
    }
}
