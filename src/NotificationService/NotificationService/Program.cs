using NotificationService;
using TaskService.Domain;
using TaskService.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<ITaskRepository, InMemoryTaskRepository>();
builder.Services.AddScoped<INotificationHandler, NotificationHandler>();

var host = builder.Build();
host.Run();
