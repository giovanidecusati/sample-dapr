namespace Nwd.Sales.Infrastructure.Data.Configuration
{
    public class CosmosDbSettings
    {
        /// <summary>
        ///     CosmosDb Account - The Azure Cosmos DB endpoint
        /// </summary>
        public string EndpointUrl { get; set; } = null!;
        /// <summary>
        ///     Key - The primary key for the Azure DocumentDB account.
        /// </summary>
        public string PrimaryKey { get; set; } = null!;
        /// <summary>
        ///     Database name
        /// </summary>
        public string DatabaseName { get; set; } = null!;

        /// <summary>
        ///     List of containers in the database
        /// </summary>
        public List<ContainerInfo> Containers { get; set; } = null!;

    }
}