using Microsoft.Azure.Cosmos;
using Nwd.Sales.Infrastructure.Data.Interfaces;

namespace Nwd.Sales.Infrastructure.Data
{
    public class CosmosDbContainer: ICosmosDbContainer
    {
        public Container _container { get; }

        public CosmosDbContainer(CosmosClient cosmosClient,
                                 string databaseName,
                                 string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }
    }
}
