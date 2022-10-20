using Microsoft.AspNetCore.Mvc;
using Nwd.Sales.Application.Commands.CreateOrder;
using Nwd.Sales.Commands.CreateOrder;

namespace Nwd.Sales.WebApi.Controllers
{
    public class OrderController : ApiControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateOrderCommandResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(object))]
        public async Task<ActionResult<CreateOrderCommandResult>> Create(CreateOrderCommand createOrderCommand)
        {
            var result = await Mediator.Send(createOrderCommand);
            return CreatedAtRoute(new { id = result.OrderId }, result);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateOrderCommandResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(object))]
        public async Task<OkResult> Get(Guid orderId)
        {
            return await Task.FromResult(Ok());
        }
    }
}
