namespace PaymentService.Consumers;

public class OrderCreatedPaymentHostedService : BackgroundService
{
    private readonly OrderCreatedPaymentConsumer _consumer;
    private readonly ILogger<OrderCreatedPaymentHostedService> _logger;

    public OrderCreatedPaymentHostedService(
        OrderCreatedPaymentConsumer consumer,
        ILogger<OrderCreatedPaymentHostedService> logger
    )
    {
        _consumer = consumer;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PaymentService consumer starting...");

        await _consumer.StartAsync(
            queueName: "order-created-payment",
            cancellationToken: stoppingToken
        );
    }
}
