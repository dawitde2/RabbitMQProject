using RabbitMQ.Client;

namespace Shared.RabbitMQ
{
    public static class RabbitMqConnection
    {
        public static IConnection GetConnection(
            string hostname = "rabbitmq",
            string username = "guest",
            string password = "guest"
        )
        {
            var factory = new ConnectionFactory
            {
                HostName = hostname,
                UserName = username,
                Password = password,
            };

            return factory.CreateConnection();
        }
    }
}
