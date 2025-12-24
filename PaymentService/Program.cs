using PaymentService.Consumers;
using Shared.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// Register RabbitMQ connection
builder.Services.AddSingleton<RabbitMqConnection>(sp =>
    RabbitMqConnection.CreateAsync(sp.GetRequiredService<IConfiguration>()).GetAwaiter().GetResult()
);

// Register the consumer
builder.Services.AddSingleton<OrderCreatedPaymentConsumer>();
builder.Services.AddSingleton<RabbitMqConnection>(sp =>
    RabbitMqConnection.CreateAsync(sp.GetRequiredService<IConfiguration>()).GetAwaiter().GetResult()
);

builder.Services.AddSingleton<OrderCreatedPaymentConsumer>();

builder.Services.AddHostedService<OrderCreatedPaymentHostedService>();

var app = builder.Build();

app.Run();
