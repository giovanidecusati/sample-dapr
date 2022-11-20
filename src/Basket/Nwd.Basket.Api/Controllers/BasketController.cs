using Dapr;
using Microsoft.AspNetCore.Mvc;
using Nwd.Basket.Api.Models;

namespace Nwd.Basket.Api.Controllers
{
    public class BasketController : ApiControllerBase
    {
        private readonly ILogger<BasketController> _logger;
        private const string STATESTORE = "statestore";
        private const string STATESTORE_KEY = "id";

        public BasketController(ILogger<BasketController> logger)
        {
            _logger = logger;
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(AcceptedResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]

        public async Task<IActionResult> Put(BasketModel basketModel, [FromState(STATESTORE, STATESTORE_KEY)] StateEntry<BasketModel> stateEntry)
        {
            if (stateEntry.Value == null)
                _logger.LogDebug("Creating basket {@basketModel}", basketModel);
            else
                _logger.LogDebug("Updating basket {@basketModel}", basketModel);

            stateEntry.Value = basketModel;

            await stateEntry.SaveAsync();

            return AcceptedAtRoute(new { id = basketModel.Id }, basketModel);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
        public ActionResult<BasketModel> Get([FromState(STATESTORE, STATESTORE_KEY)] StateEntry<BasketModel> stateEntry)
        {
            if (stateEntry.Value == null)
            {
                return NotFound();
            }

            return stateEntry.Value;
        }
    }
}
