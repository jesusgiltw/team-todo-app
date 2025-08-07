using Microsoft.AspNetCore.Mvc;
using Shared.Events;
using Shared.Messaging;
using TaskService.API;
using TaskService.Domain;

namespace TaskService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskRepository _repository;
    private readonly INotificationPublisher _publisher;

    public TasksController(ITaskRepository repository, INotificationPublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll()
    {
        var tasks = await _repository.GetAllAsync();
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItem>> GetById(Guid id)
    {
        var task = await _repository.GetByIdAsync(id);
        return task is not null ? Ok(task) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateTaskRequest request)
    {
        if (request.DueDate <= DateTime.UtcNow)
        {
            return BadRequest("La fecha de vencimiento debe estar en el futuro.");
        }

        var task = new TaskItem(request.Title, request.DueDate);
        await _repository.AddAsync(task);

        var notification = new TaskCreatedNotification(task.Id, task.Title, task.DueDate);
        await _publisher.PublishAsync(notification);

        if (task.DueDate <= DateTime.UtcNow.AddHours(24))
        {
            await _publisher.PublishAsync(
                new TaskDueSoonNotification(task.Id, task.Title, task.DueDate)
            );
        }

        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }


    [HttpPost("{id}/complete")]
    public async Task<ActionResult> Complete(Guid id)
    {
        var task = await _repository.GetByIdAsync(id);
        if (task == null)
            return NotFound();

        task.Complete();
        await _repository.UpdateAsync(task);

        return Ok(task);
    }

    [HttpDelete("{id}/delete")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var task = await _repository.GetByIdAsync(id);
        if (task == null)
            return NotFound();

        await _repository.DeleteAsync(id);
        return Ok(task);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] UpdateTaskRequest request)
    {
        if(request.Title is null || request.Title.Trim().Length == 0)
        {
            return BadRequest("El título no puede estar vacío.");
        }
        var task = await _repository.GetByIdAsync(id);
        if (task == null)
            return NotFound();

        task.UpdateTitle(request.Title);
        task.UpdateDueDate(request.DueDate);
        await _repository.UpdateAsync(task);

        return Ok(task);
    }
}
