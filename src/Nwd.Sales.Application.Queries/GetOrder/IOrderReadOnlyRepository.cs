namespace Nwd.Sales.Application.Queries.GetOrder
{
    public interface IOrderReadOnlyRepository
    {
        Task<GetSingleOrderQueryResult> GetSingleOrder(GetSingleOrderQuery request);
    }
}
