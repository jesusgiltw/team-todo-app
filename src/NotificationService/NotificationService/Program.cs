using NotificationService.Handlers;
using Shared.Messaging;
using Shared.Events;

var builder = Host.CreateApplicationBuilder(args);

var publisher = new InMemoryNotificationPublisher();
builder.Services.AddSingleton<INotificationPublisher>(publisher);

var handler = new TaskDueSoonHandler();
publisher.Subscribe<TaskDueSoonNotification>(handler.Handle);

var host = builder.Build();
host.Run();
