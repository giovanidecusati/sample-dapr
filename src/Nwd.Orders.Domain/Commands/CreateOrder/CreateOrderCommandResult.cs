namespace Nwd.Orders.Domain.Commands.CreateOrder
{
    public class CreateOrderCommandResult
    {
        public CreateOrderCommandResult(string orderId)
        {
            OrderId = orderId;
        }

        public string OrderId { get; private set; }
    }
}
