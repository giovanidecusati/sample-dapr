using Microsoft.AspNetCore.Mvc;
using Nwd.Sales.Application.Commands;
using Nwd.Sales.Application.Commands.CreateOrder;
using Nwd.Sales.Commands.CreateOrder;

namespace Nwd.Sales.WebApi.Controllers
{
    public class OrderController : ApiControllerBase
    {
        private readonly OrderCommandHandler _orderCommandHandler;

        public OrderController(OrderCommandHandler orderCommandHandler)
        {
            _orderCommandHandler = orderCommandHandler;
        }

        [HttpPost]
        public async Task<ActionResult<CreateOrderCommandResult>> Create(CreateOrderCommand createOrderCommand)
        {
            return await _orderCommandHandler.CreateOrder(createOrderCommand);
        }
    }
}
