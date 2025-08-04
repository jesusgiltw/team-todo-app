using TaskService.Domain;
using TaskService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ITaskRepository, InMemoryTaskRepository>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();