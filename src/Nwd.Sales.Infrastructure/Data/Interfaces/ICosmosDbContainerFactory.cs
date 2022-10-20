namespace Nwd.Sales.Infrastructure.Data.Interfaces
{
    public interface ICosmosDbContainerFactory
    {
        /// <summary>
        ///     Returns a CosmosDbContainer wrapper
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns></returns>
        ICosmosDbContainer GetContainer(string containerName);

        /// <summary>
        ///     Ensure the database is created
        /// </summary>
        /// <returns></returns>
        Task EnsureDbSetupAsync();

        /// <summary>
        ///     Check health
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task CheckHealthAsync(CancellationToken cancellationToken = default);
    }
}
