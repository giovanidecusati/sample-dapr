namespace Nwd.Orders.Application.Queries.GetSingleOrder
{
    public interface IGetSingleOrderReadOnlyRepository
    {
        Task<GetSingleOrderQueryResult> GetSingleOrderAsync(GetSingleOrderQuery request, CancellationToken cancellationToken = default);
    }
}
