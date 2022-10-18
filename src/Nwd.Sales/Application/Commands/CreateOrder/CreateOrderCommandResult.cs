namespace Nwd.Sales.Application.Commands.CreateOrder
{
    public class CreateOrderCommandResult
    {
        public CreateOrderCommandResult(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; private set; }
    }
}
