namespace TaskService.Domain;

public interface ITaskRepository
{
    Task AddAsync(TaskItem task);
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task<IEnumerable<TaskItem>> GetAllAsync();
    Task UpdateAsync(TaskItem task);
    Task DeleteAsync(Guid id);
}
