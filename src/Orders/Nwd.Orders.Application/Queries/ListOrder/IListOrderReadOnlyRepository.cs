namespace Nwd.Orders.Application.Queries.ListOrder
{
    public interface IListOrderReadOnlyRepository
    {
        Task<IList<ListOrderQueryResult>> ListOrderAsync(ListOrderQuery request, CancellationToken cancellationToken = default);
    }
}
