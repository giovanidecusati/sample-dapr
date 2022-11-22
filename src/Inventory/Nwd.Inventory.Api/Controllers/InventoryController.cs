using Microsoft.AspNetCore.Mvc;
using Nwd.Inventory.Application.Commands.UpdateInventory;

namespace Nwd.Inventory.Api.Controllers
{
    public class InventoryController : ApiControllerBase
    {
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(ILogger<InventoryController> logger)
        {
            _logger = logger;
        }

        [HttpPut("increase")]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(UpdateInventoryCommandResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> Increase(IncreaseInventoryCommand increaseInventoryCommand)
        {
            var result = await Mediator.Send(increaseInventoryCommand);
            return AcceptedAtRoute(new { Id = result.ProductId }, result);
        }

        [HttpPut("decrease")]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(UpdateInventoryCommandResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> Decrease(DecreaseInventoryCommand decreaseInventoryCommand)
        {
            var result = await Mediator.Send(decreaseInventoryCommand);
            return AcceptedAtRoute(new { Id = result.ProductId }, result);
        }
    }
}
