using Microsoft.AspNetCore.Mvc;
using TaskService.API;
using TaskService.Domain;

namespace TaskService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskRepository _repository;

    public TasksController(ITaskRepository repository)
    {
        _repository = repository;
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
        var task = new TaskItem(request.Title, request.DueDate);
        await _repository.AddAsync(task);
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

        return NoContent();
    }
}
