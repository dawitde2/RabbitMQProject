using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Events;
using Shared.RabbitMQ;

public class OrderCreatedInventoryConsumer
{
    private readonly RabbitMqConnection _rabbitMq;
    private readonly ILogger<OrderCreatedInventoryConsumer> _logger;

    public OrderCreatedInventoryConsumer(
        RabbitMqConnection rabbitMq,
        ILogger<OrderCreatedInventoryConsumer> logger
    )
    {
        _rabbitMq = rabbitMq;
        _logger = logger;
    }

    public async Task StartAsync(string queueName, CancellationToken cancellationToken)
    {
        var channel = await _rabbitMq.CreateChannelAsync;

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false
        );

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (_, args) =>
        {
            var json = Encoding.UTF8.GetString(args.Body.ToArray());
            var order = JsonSerializer.Deserialize<OrderCreatedEvent>(json);

            _logger.LogInformation(
                "[Inventory] Reserving {Qty} of {Product}",
                order!.Quantity,
                order.ProductName
            );

            await channel.BasicAckAsync(args.DeliveryTag, false);
        };

        await channel.BasicConsumeAsync(queueName, false, consumer);

        await Task.Delay(Timeout.Infinite, cancellationToken);
    }
}
