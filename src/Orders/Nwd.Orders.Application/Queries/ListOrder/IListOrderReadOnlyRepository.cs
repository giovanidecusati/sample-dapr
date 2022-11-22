namespace Nwd.Orders.Application.Queries.ListOrder
{
    public interface IListOrderReadOnlyRepository
    {
        Task<IList<ListOrderQueryResult>> ListOrder(ListOrderQuery request);
    }
}
