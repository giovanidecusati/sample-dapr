using Dapr.Client;
using MediatR;
using Nwd.Inventory.Application.Commands.CreateProduct;

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
            await _daprClient.PublishEventAsync("queue-component", "new-product-topic", productCreatedEvent);
        }
    }
}
