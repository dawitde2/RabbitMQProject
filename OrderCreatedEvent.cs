using System;

namespace Shared.Events
{
    public class OrderCreatedEvent
    {
        public Guid OrderId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public OrderCreatedEvent() { }

        public OrderCreatedEvent(Guid orderId, string productName, int quantity, decimal price)
        {
            OrderId = orderId;
            ProductName = productName;
            Quantity = quantity;
            Price = price;
        }
    }
}
