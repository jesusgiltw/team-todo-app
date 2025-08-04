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

    public Task UpdateAsync(TaskItem task)
    {
        if (_storage.ContainsKey(task.Id))
        {
            _storage[task.Id] = task;
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _storage.TryRemove(id, out _);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<TaskItem>> GetTasksDueBetweenAsync(DateTime start, DateTime end, CancellationToken cancellationToken)
    {
        var tasksDue = _storage.Values
            .Where(task => task.DueDate >= start && task.DueDate <= end && !task.IsCompleted);
        return Task.FromResult(tasksDue.AsEnumerable());
    }
}
