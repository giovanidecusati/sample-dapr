using FluentValidation;
using Nwd.Sales.Application.Commands.CreateOrder;
using Nwd.Sales.Application.Queries.GetOrderStatus;
using Nwd.Sales.Commands.CreateOrder;
using Nwd.Sales.Domain.Orders;

namespace Nwd.Sales.Application.Commands
{
    public class OrderCommandHandler
    {
        public readonly IOrderRepository _orderRepository;
        public readonly IProductRepository _productRepository;
        public readonly CreateOrderValidator _createOrderValidation;

        public OrderCommandHandler(IOrderRepository orderRepository, CreateOrderValidator createOrderValidation, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _createOrderValidation = createOrderValidation;
        }

        public async Task<CreateOrderCommandResult> CreateOrder(CreateOrderCommand createOrderCommand)
        {
            await _createOrderValidation.ValidateAndThrowAsync(createOrderCommand);

            var order = new OrderAgg(createOrderCommand.CustomerId);
            createOrderCommand.Items.ForEach(async item =>
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                order.AddItem(product, item.Quantity);
            });

            await _orderRepository.SaveAsync(order);
            return new CreateOrderCommandResult(order.Id);
        }

        public async Task<GetOrderQueryResult> GetOrder(Guid orderId)
        {
            return await Task.FromResult(new GetOrderQueryResult());
        }
    }
}
