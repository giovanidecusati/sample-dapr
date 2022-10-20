using Microsoft.AspNetCore.Mvc;
using Nwd.Sales.Application.Commands.CreateOrder;
using Nwd.Sales.Commands.CreateOrder;

namespace Nwd.Sales.WebApi.Controllers
{
    public class OrderController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<CreateOrderCommandResult>> Create(CreateOrderCommand createOrderCommand)
        {
            return await Mediator.Send(createOrderCommand);
        }
    }
}
