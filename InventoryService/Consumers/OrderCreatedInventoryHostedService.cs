public class OrderCreatedInventoryHostedService : BackgroundService
{
    private readonly OrderCreatedInventoryConsumer _consumer;
    private readonly ILogger<OrderCreatedInventoryHostedService> _logger;

    public OrderCreatedInventoryHostedService(
        OrderCreatedInventoryConsumer consumer,
        ILogger<OrderCreatedInventoryHostedService> logger
    )
    {
        _consumer = consumer;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Inventory consumer starting...");

        await _consumer.StartAsync(
            queueName: "order-created-inventory",
            cancellationToken: stoppingToken
        );
    }
}
