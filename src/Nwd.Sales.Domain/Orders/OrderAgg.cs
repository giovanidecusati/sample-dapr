using FluentValidation;
using Nwd.Sales.Domain.Common;

namespace Nwd.Sales.Domain.Orders
{
    public class OrderAgg : BaseEntity
    {
        private readonly IValidator<OrderAgg> _validator;

        private OrderAgg() { }

        public OrderAgg(IValidator<OrderAgg> validator, Customer customer, Address shipTo, OrderItemCollection orderItemCollection)
        {
            _ = validator ?? throw new ArgumentNullException(nameof(validator));
            _ = customer ?? throw new ArgumentNullException(nameof(customer));
            _ = shipTo ?? throw new ArgumentNullException(nameof(shipTo));
            _ = orderItemCollection ?? throw new ArgumentNullException(nameof(orderItemCollection));

            _validator = validator;
            CustomerId = customer.Id;
            ShipTo = shipTo;
            Status = OrderStatus.Processing;
            Items = new List<OrderItem>();

            AddItems(orderItemCollection);

            _validator.ValidateAndThrow(this);
        }

        private void AddItems(OrderItemCollection orderItemCollection)
        {
            foreach (var item in orderItemCollection)
            {
                Items.Add(new OrderItem(this, item.Key, item.Value));
            }
        }

        public string CustomerId { get; private set; }

        public List<OrderItem> Items { get; private set; }

        public OrderStatus Status { get; private set; }

        public Address ShipTo { get; private set; }
    }
}