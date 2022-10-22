namespace Nwd.Orders.Domain.Queries.GetSingleOrder
{
    public interface IOrderReadOnlyRepository
    {
        Task<GetSingleOrderQueryResult> GetSingleOrder(GetSingleOrderQuery request);
    }
}
