using Shared.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<RabbitMqPublisher>();

builder.Services.AddSingleton<RabbitMqConnection>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return RabbitMqConnection.CreateAsync(config).GetAwaiter().GetResult();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost(
    "/orders",
    static async (RabbitMqPublisher publisher) =>
    {
        int i = 50;
        await publisher.PublishAsync(
            new
            {
                OrderId = Guid.NewGuid(),
                ProductName = "Laptop",
                Quantity = 1,
            },
            "order-created"
        );

        return Results.Ok();
    }
);

app.Run();
