using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Events;
using Shared.RabbitMQ;

namespace PaymentService.Consumers;

public class OrderCreatedPaymentConsumer
{
    private readonly RabbitMqConnection _rabbitMq;
    private readonly ILogger<OrderCreatedPaymentConsumer> _logger;

    public OrderCreatedPaymentConsumer(
        RabbitMqConnection rabbitMq,
        ILogger<OrderCreatedPaymentConsumer> logger
    )
    {
        _rabbitMq = rabbitMq;
        _logger = logger;
    }

    public async Task StartAsync(string queueName, CancellationToken cancellationToken)
    {
        var channel = await _rabbitMq.CreateChannelAsync;

        await channel.ExchangeDeclareAsync(
            exchange: "order-created",
            type: ExchangeType.Fanout,
            durable: true
        );

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false
        );

        await channel.QueueBindAsync(queue: queueName, exchange: "order-created", routingKey: "");

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (_, args) =>
        {
            var json = Encoding.UTF8.GetString(args.Body.ToArray());
            var order = JsonSerializer.Deserialize<OrderCreatedEvent>(json);

            _logger.LogInformation("[Payment] Charging for {Product}", order!.ProductName);

            await channel.BasicAckAsync(args.DeliveryTag, false);
        };

        await channel.BasicConsumeAsync(queueName, false, consumer);
    }
}
