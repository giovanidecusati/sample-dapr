namespace Nwd.Orders.Domain.Queries.ListOrder
{
    public interface IListOrderReadOnlyRepository
    {
        Task<IList<ListOrderQueryResult>> ListOrder(ListOrderQuery request);
    }
}
