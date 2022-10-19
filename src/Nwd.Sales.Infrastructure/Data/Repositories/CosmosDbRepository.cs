﻿using Microsoft.Azure.Cosmos;
using Nwd.Sales.Domain.Common;
using Nwd.Sales.Infrastructure.Data.Interfaces;

namespace Nwd.Sales.Infrastructure.Data.Repositories
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
        internal readonly Microsoft.Azure.Cosmos.Container _container;


        public CosmosDbRepository(ICosmosDbContainerFactory cosmosDbContainerFactory)
        {
            this._cosmosDbContainerFactory = cosmosDbContainerFactory ?? throw new ArgumentNullException(nameof(ICosmosDbContainerFactory));
            this._container = this._cosmosDbContainerFactory.GetContainer(ContainerName)._container;
        }

        public virtual async Task AddAsync(T item)
        {
            var id = GenerateId(item);
            await _container.CreateItemAsync<T>(item, ResolvePartitionKey(id));
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