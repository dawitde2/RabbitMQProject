using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Shared.RabbitMQ;

public class RabbitMqPublisher
{
    private readonly RabbitMqConnection _rabbitMq;

    public RabbitMqPublisher(RabbitMqConnection rabbitMq)
    {
        _rabbitMq = rabbitMq;
    }

    public async Task PublishAsync<T>(T message, string exchangeName)
    {
        var channel = await _rabbitMq.CreateChannelAsync;

        await channel.ExchangeDeclareAsync(
            exchange: exchangeName,
            type: ExchangeType.Fanout,
            durable: true
        );

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        await channel.BasicPublishAsync(
            exchange: exchangeName,
            routingKey: "order.created",
            body: body
        );
    }
}
