using TaskService.Domain;
using TaskService.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Shared.Messaging;
using TaskService.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inyección del repo en memoria
builder.Services.AddSingleton<ITaskRepository, InMemoryTaskRepository>();
builder.Services.AddSingleton<INotificationPublisher, InMemoryNotificationPublisher>();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
