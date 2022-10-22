using Microsoft.Azure.Cosmos;
using Nwd.Orders.Domain.Entities;
using Nwd.Orders.Infrastructure.Data.Interfaces;

namespace Nwd.Orders.Infrastructure.Data.Repositories
{
    public abstract class CosmosDbRepository<T> where T : BaseEntity
    {
        /// <summary>
        ///     Name of the CosmosDB container
        /// </summary>
        public abstract string ContainerName { get; }

        /// <summary>
        ///     Generate id
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public abstract string GenerateId(T entity);

        /// <summary>
        ///     Resolve the partition key
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public abstract PartitionKey ResolvePartitionKey(string entityId);

        /// <summary>
        ///     Cosmos DB factory
        /// </summary>
        private readonly ICosmosDbContainerFactory _cosmosDbContainerFactory;

        /// <summary>
        ///     Cosmos DB container
        /// </summary>
        private readonly Microsoft.Azure.Cosmos.Container _container;

        public CosmosDbRepository(ICosmosDbContainerFactory cosmosDbContainerFactory)
        {
            _cosmosDbContainerFactory = cosmosDbContainerFactory ?? throw new ArgumentNullException(nameof(ICosmosDbContainerFactory));
            _container = _cosmosDbContainerFactory.GetContainer(ContainerName)._container;
        }

        public virtual async Task AddAsync(T item)
        {
            item.Id = GenerateId(item);
            await _container.CreateItemAsync<T>(item, ResolvePartitionKey(item.Id));
        }

        public async Task DeleteAsync(string id)
        {
            await this._container.DeleteItemAsync<T>(id.ToString(), ResolvePartitionKey(id));
        }

        public virtual async Task<T> GetByIdAsync(string id)
        {
            try
            {
                ItemResponse<T> response = await _container.ReadItemAsync<T>(id, ResolvePartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }
    }
}