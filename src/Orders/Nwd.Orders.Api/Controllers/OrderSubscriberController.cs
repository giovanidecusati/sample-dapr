using Dapr;
using Microsoft.AspNetCore.Mvc;
using Nwd.Orders.Application.Commands.ProcessOrder;
using Nws.BuildingBlocks.Events;

namespace Nwd.Orders.Api.Controllers
{
    public class OrderSubscriberController : ApiControllerBase
    {
        [Topic("queue-component", "order-submitted-topic")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(ProcessOrderCommandResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedObjectResult))]
        public async Task<ActionResult<ProcessOrderCommandResult>> Create(OrderSubmittedEvent processOrderCommand)
        {
            var result = await Mediator.Send(new ProcessOrderCommand() { OrderId = processOrderCommand.OrderId });
            return AcceptedAtRoute(new { id = result.OrderId, routeName = "order" }, result);
        }
    }
}