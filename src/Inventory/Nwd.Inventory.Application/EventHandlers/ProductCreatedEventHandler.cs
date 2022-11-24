using Dapr.Client;
using MediatR;
using Nws.BuildingBlocks;
using Nws.BuildingBlocks.Events;

namespace Nwd.Inventory.Application.EventHandlers
{
    public class ProductCreatedEventHandler : INotificationHandler<ProductCreatedEvent>
    {
        private readonly DaprClient _daprClient;

        public ProductCreatedEventHandler(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        public async Task Handle(ProductCreatedEvent productCreatedEvent, CancellationToken cancellationToken)
        {
            // Publish an event/message using Dapr PubSub
            await _daprClient.PublishEventAsync(DaprConstants.DAPR_PUBSUB_NAME, nameof(ProductCreatedEvent), productCreatedEvent);
        }
    }
}
