namespace Nwd.Orders.Infrastructure.Data.Configuration
{
    public class ContainerInfo
    {
        /// <summary>
        ///     Container Name
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        ///     Container partition Key
        /// </summary>
        public string PartitionKey { get; set; } = null!;
    }

}