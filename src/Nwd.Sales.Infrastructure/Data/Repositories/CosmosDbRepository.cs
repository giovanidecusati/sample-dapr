using AutoMapper;
using Microsoft.Azure.Cosmos;
using Nwd.Sales.Domain.Common;
using Nwd.Sales.Infrastructure.Data.Interfaces;

namespace Nwd.Sales.Infrastructure.Data.Repositories
{
    public abstract class CosmosDbRepository<Tdomain, Tentity> where Tdomain : BaseEntity where Tentity : class
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
        public abstract string GenerateId(Tdomain entity);

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

        private readonly IMapper _mapper;

        public CosmosDbRepository(ICosmosDbContainerFactory cosmosDbContainerFactory, IMapper mapper)
        {
            this._cosmosDbContainerFactory = cosmosDbContainerFactory ?? throw new ArgumentNullException(nameof(ICosmosDbContainerFactory));
            this._container = this._cosmosDbContainerFactory.GetContainer(ContainerName)._container;
            _mapper = mapper;
        }

        public virtual async Task AddAsync(Tdomain item)
        {
            var id = GenerateId(item);
            await _container.CreateItemAsync<Tentity>(_mapper.Map<Tentity>(item), ResolvePartitionKey(id));
        }

        public async Task DeleteAsync(string id)
        {
            await this._container.DeleteItemAsync<Tentity>(id.ToString(), ResolvePartitionKey(id));
        }

        public virtual async Task<Tdomain> GetByIdAsync(string id)
        {
            try
            {
                ItemResponse<Tentity> response = await _container.ReadItemAsync<Tentity>(id, ResolvePartitionKey(id));
                return _mapper.Map<Tdomain>(response.Resource);
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }
    }
}