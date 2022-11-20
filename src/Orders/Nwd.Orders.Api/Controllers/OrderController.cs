using Microsoft.AspNetCore.Mvc;
using Nwd.Orders.Commands.CreateOrder;
using Nwd.Orders.Domain.Commands.CreateOrder;
using Nwd.Orders.Domain.Queries.GetSingleOrder;
using Nwd.Orders.Domain.Queries.ListOrder;

namespace Nwd.Orders.Api.Controllers
{
    public class OrderController : ApiControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateOrderCommandResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedObjectResult))]
        public async Task<ActionResult<CreateOrderCommandResult>> Create(CreateOrderCommand createOrderCommand)
        {
            var result = await Mediator.Send(createOrderCommand);
            return CreatedAtRoute(new { id = result.OrderId }, result);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetSingleOrderQueryResult))]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(NoContentResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedObjectResult))]
        public async Task<GetSingleOrderQueryResult> Get([FromQuery] GetSingleOrderQuery getSingleOrderQuery)
        {
            return await Mediator.Send(getSingleOrderQuery);
        }

        [HttpGet("SearchBy")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<ListOrderQueryResult>))]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(NoContentResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedObjectResult))]
        public async Task<IList<ListOrderQueryResult>> SearchBy([FromQuery] ListOrderQuery listOrderQuery)
        {
            return await Mediator.Send(listOrderQuery);
        }
    }
}
