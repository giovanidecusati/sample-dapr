using AutoMapper;
using Microsoft.Azure.Cosmos;
using Nwd.Sales.Application.Queries.GetOrder;
using Nwd.Sales.Infrastructure.Data.Interfaces;

namespace Nwd.Sales.Infrastructure.Data.Repositories
{
    public class OrderReadOnlyRepository : IOrderReadOnlyRepository
    {
        private readonly Microsoft.Azure.Cosmos.Container _container;
        private readonly ICosmosDbContainerFactory _cosmosDbContainerFactory;
        private readonly IMapper _mapper;

        public string ContainerName { get; } = "Orders";

        public OrderReadOnlyRepository(ICosmosDbContainerFactory cosmosDbContainerFactory, IMapper mapper)
        {
            _cosmosDbContainerFactory = cosmosDbContainerFactory ?? throw new ArgumentNullException(nameof(ICosmosDbContainerFactory));
            _container = this._cosmosDbContainerFactory.GetContainer(ContainerName)._container;
            _mapper = mapper;
        }


        public async Task<GetSingleOrderQueryResult> GetSingleOrder(GetSingleOrderQuery request)
        {
            string query = @$"SELECT * FROM c WHERE c.customerId = @customerId AND c.id = @orderId";
            QueryDefinition queryDefinition = new QueryDefinition(query)
                .WithParameter("@orderId", request.OrderId)
                .WithParameter("@customerId", request.CustomerId);

            var resultSetIterator = _container.GetItemQueryIterator<Entities.Order>(queryDefinition);
            while (resultSetIterator.HasMoreResults)
            {
                var response = await resultSetIterator.ReadNextAsync();
                var items = response.ToList();
                return _mapper.Map<GetSingleOrderQueryResult>(items.FirstOrDefault());
            }

            return null;
        }
    }
}
