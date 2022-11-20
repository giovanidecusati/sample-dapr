using Microsoft.Azure.Cosmos;
using Nwd.Orders.Infrastructure.Data.Interfaces;

namespace Nwd.Orders.Infrastructure.Data
{
    public class CosmosDbContainer : ICosmosDbContainer
    {
        public static readonly string OrdersContainerName = "Orders";
        public static readonly string ProductsContainerName = "Products";
        public static readonly string CustomersContainerName = "Customers";

        public Container _container { get; }

        public CosmosDbContainer(CosmosClient cosmosClient,
                                 string databaseName,
                                 string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }
    }
}
