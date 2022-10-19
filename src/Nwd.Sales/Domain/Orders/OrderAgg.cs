using FluentValidation;
using Newtonsoft.Json;

namespace Nwd.Sales.Domain.Orders
{
    public class OrderAgg
    {
        private readonly IValidator<OrderAgg> _validator;

        public OrderAgg(IValidator<OrderAgg> validator, Customer customer, Address shipTo, OrderItemCollection orderItemCollection)
        {
            _ = validator ?? throw new ArgumentNullException(nameof(validator));
            _ = customer ?? throw new ArgumentNullException(nameof(customer));
            _ = shipTo ?? throw new ArgumentNullException(nameof(shipTo));
            _ = orderItemCollection ?? throw new ArgumentNullException(nameof(orderItemCollection));

            _validator = validator;
            Id = Guid.NewGuid();
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

        [JsonProperty("id")]
        public Guid Id { get; private set; }

        [JsonProperty("customerId")]
        public Guid CustomerId { get; private set; }

        [JsonProperty("items")]
        public List<OrderItem> Items { get; private set; }

        [JsonProperty("status")]
        public OrderStatus Status { get; private set; }

        [JsonProperty("shipTo")]
        public Address ShipTo { get; private set; }
    }
}