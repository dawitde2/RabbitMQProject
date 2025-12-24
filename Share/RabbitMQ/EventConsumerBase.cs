using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Shared.RabbitMQ;

public abstract class EventConsumerBase<T>
{
    protected readonly RabbitMqConnection _rabbitMq;
    private IChannel? _channel;

    protected EventConsumerBase(RabbitMqConnection rabbitMq)
    {
        _rabbitMq = rabbitMq;
    }

    protected abstract Task HandleMessage(T message);

    public async Task StartAsync(string queueName, string exchangeName)
    {
        _channel = await _rabbitMq.CreateChannelAsync;

        // 1️⃣ Exchange
        await _channel.ExchangeDeclareAsync(
            exchange: exchangeName,
            type: ExchangeType.Topic,
            durable: true
        );

        // 2️⃣ Queue
        await _channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false
        );

        // 3️⃣ Binding
        await _channel.QueueBindAsync(
            queue: queueName,
            exchange: exchangeName,
            routingKey: "order.created"
        );

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (_, args) =>
        {
            try
            {
                var json = Encoding.UTF8.GetString(args.Body.ToArray());
                var message = JsonSerializer.Deserialize<T>(json);

                if (message != null)
                    await HandleMessage(message);

                await _channel.BasicAckAsync(args.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Consumer error: {ex}");
            }
        };

        await _channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);

        Console.WriteLine($"✅ {queueName} subscribed to {exchangeName}");
    }
}
