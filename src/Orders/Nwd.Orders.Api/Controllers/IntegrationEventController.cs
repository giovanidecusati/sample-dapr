using Dapr;
using Microsoft.AspNetCore.Mvc;
using Nwd.Orders.Application.Commands.ProcessOrder;
using Nws.BuildingBlocks;
using Nws.BuildingBlocks.Events;

namespace Nwd.Orders.Api.Controllers
{
    public class IntegrationEventController : ApiControllerBase
    {
        [Topic(DaprConstants.DAPR_PUBSUB_NAME, nameof(OrderSubmittedEvent))]
        [HttpPost("OrderSubmitted")] // must be Post for Dapr subscribers
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(ProcessOrderCommandResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedObjectResult))]
        public async Task HandleAsync(OrderSubmittedEvent orderSubmittedEvent)
            => await Mediator.Send(new ProcessOrderCommand() { OrderId = orderSubmittedEvent.OrderId });

        [Topic(DaprConstants.DAPR_PUBSUB_NAME, nameof(ProductCreatedEvent))]
        [HttpPost("ProductCreated")] // must be Post for Dapr subscribers
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(AcceptedResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedObjectResult))]
        public async Task Subscribe(ProductCreatedEvent productCreatedEvent)
            => await Mediator.Publish(productCreatedEvent);
    }
}