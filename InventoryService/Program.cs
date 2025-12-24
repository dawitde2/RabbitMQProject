using Shared.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// Register RabbitMQ connection
builder.Services.AddSingleton<RabbitMqConnection>(sp =>
    RabbitMqConnection.CreateAsync(sp.GetRequiredService<IConfiguration>()).GetAwaiter().GetResult()
);

// Register the consumer
builder.Services.AddSingleton<OrderCreatedInventoryConsumer>();
builder.Services.AddSingleton<RabbitMqConnection>(sp =>
    RabbitMqConnection.CreateAsync(sp.GetRequiredService<IConfiguration>()).GetAwaiter().GetResult()
);

builder.Services.AddSingleton<OrderCreatedInventoryConsumer>();

builder.Services.AddHostedService<OrderCreatedInventoryHostedService>();

var app = builder.Build();

app.Run();
