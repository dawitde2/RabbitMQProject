using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Shared.RabbitMQ;

public class RabbitMqConnection : IDisposable
{
    private readonly IConnection _connection;

    private RabbitMqConnection(IConnection connection)
    {
        _connection = connection;
    }

    public static async Task<RabbitMqConnection> CreateAsync(IConfiguration config)
    {
        var factory = new ConnectionFactory
        {
            HostName = config["RabbitMQ:Host"],
            Port = int.Parse(config["RabbitMQ:Port"]!),
            UserName = config["RabbitMQ:UserName"],
            Password = config["RabbitMQ:Password"],
        };

        var connection = await factory.CreateConnectionAsync();
        return new RabbitMqConnection(connection);
    }

    public Task<IChannel> CreateChannelAsync => _connection.CreateChannelAsync();

    public void Dispose()
    {
        _connection.Dispose();
    }
}
