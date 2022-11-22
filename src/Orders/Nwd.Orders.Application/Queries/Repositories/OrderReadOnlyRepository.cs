using Microsoft.Azure.Cosmos;
using Nwd.Orders.Application.Queries.GetSingleOrder;
using Nwd.Orders.Application.Queries.ListOrder;
using Nwd.Orders.Infrastructure.Data;
using Nwd.Orders.Infrastructure.Data.Interfaces;

namespace Nwd.Orders.Application.Queries.Repositories
{
    public class OrderReadOnlyRepository : IOrderReadOnlyRepository, IListOrderReadOnlyRepository
    {
        private readonly Microsoft.Azure.Cosmos.Container _container;
        private readonly ICosmosDbContainerFactory _cosmosDbContainerFactory;

        public string ContainerName { get; } = CosmosDbContainer.OrdersContainerName;

        public OrderReadOnlyRepository(ICosmosDbContainerFactory cosmosDbContainerFactory)
        {
            _cosmosDbContainerFactory = cosmosDbContainerFactory ?? throw new ArgumentNullException(nameof(ICosmosDbContainerFactory));
            _container = _cosmosDbContainerFactory.GetContainer(ContainerName)._container;
        }


        public async Task<GetSingleOrderQueryResult> GetSingleOrder(GetSingleOrderQuery request)
        {
            string query = @$"SELECT * FROM c WHERE c.customer.id = @customerId AND c.id = @orderId";
            QueryDefinition queryDefinition = new QueryDefinition(query)
                .WithParameter("@orderId", request.OrderId)
                .WithParameter("@customerId", request.CustomerId);

            var resultSetIterator = _container.GetItemQueryIterator<GetSingleOrderQueryResult>(queryDefinition);
            while (resultSetIterator.HasMoreResults)
            {
                var response = await resultSetIterator.ReadNextAsync();
                var items = response.ToList();
                return items.FirstOrDefault();
            }

            return null;
        }

        // TODO
        // Search data using SQL query string
        // This shows how to use SQL string to read data from Cosmos DB for demonstration purpose.
        // For production, try to use safer alternatives like Parameterized Query and LINQ if possible.
        // Using string can expose SQL Injection vulnerability, e.g. select * from c where c.id=1 OR 1=1. 
        // String can also be hard to work with due to special characters and spaces when advanced querying like search and pagination is required.
        public async Task<IList<ListOrderQueryResult>> ListOrder(ListOrderQuery request)
        {
            string query = @$"SELECT c.id as orderId, c.createdAt, c.status, c.customer.id as customerId, c.customer.name as customerName FROM c WHERE c.customer.id = @customerId ORDER BY c.createdAt";
            QueryDefinition queryDefinition = new QueryDefinition(query)
                .WithParameter("@customerId", request.CustomerId);

            var results = new List<ListOrderQueryResult>();
            var resultSetIterator = _container.GetItemQueryIterator<ListOrderQueryResult>(queryDefinition);
            while (resultSetIterator.HasMoreResults)
            {
                FeedResponse<ListOrderQueryResult> response = await resultSetIterator.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }
    }
}
