using Dapr.Client;
using Nwd.Orders.Application.Queries.GetSingleOrder;
using Nwd.Orders.Application.Queries.ListOrder;

namespace Nwd.Orders.Application.Queries.Repositories
{
    public class OrderReadOnlyRepository : IGetSingleOrderReadOnlyRepository, IListOrderReadOnlyRepository
    {
        private readonly DaprClient _client;

        public string StoreName { get; } = "order";

        public OrderReadOnlyRepository(DaprClient client)
        {
            _client = client;
        }


        public async Task<GetSingleOrderQueryResult> GetSingleOrderAsync(GetSingleOrderQuery request, CancellationToken cancellationToken = default)
        {
            string query = @$"SELECT * FROM c WHERE c.customer.id = @customerId AND c.id = @orderId";
            var result = await _client.QueryStateAsync<GetSingleOrderQueryResult>(StoreName, query, cancellationToken: cancellationToken);
            var order = result.Results.FirstOrDefault();
            return order.Data;
        }

        public async Task<IList<ListOrderQueryResult>> ListOrderAsync(ListOrderQuery request, CancellationToken cancellationToken = default)
        {
            string query = @$"SELECT c.id as orderId, c.createdAt, c.status, c.customer.id as customerId, c.customer.name as customerName FROM c WHERE c.customer.id = @customerId ORDER BY c.createdAt";

            var result = await _client.QueryStateAsync<ListOrderQueryResult>(StoreName, query, cancellationToken: cancellationToken);

            return result.Results.Select(i => i.Data).ToList();
        }
    }
}
