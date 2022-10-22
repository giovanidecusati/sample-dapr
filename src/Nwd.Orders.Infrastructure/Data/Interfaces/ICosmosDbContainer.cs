using Microsoft.Azure.Cosmos;

namespace Nwd.Orders.Infrastructure.Data.Interfaces
{
    public interface ICosmosDbContainer
    {
        /// <summary>
        ///     Instance of Azure Cosmos DB Container class
        /// </summary>
        Container _container { get; }
    }
}
