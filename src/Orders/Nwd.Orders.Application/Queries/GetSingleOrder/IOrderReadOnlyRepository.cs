namespace Nwd.Orders.Application.Queries.GetSingleOrder
{
    public interface IOrderReadOnlyRepository
    {
        Task<GetSingleOrderQueryResult> GetSingleOrder(GetSingleOrderQuery request);
    }
}
