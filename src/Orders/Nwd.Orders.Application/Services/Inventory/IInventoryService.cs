namespace Nwd.Orders.Application.Services.Inventory
{
    public interface IInventoryService
    {
        Task UpdateAsync(UpdateInventoryModel decreaseInventoryModel, string accessToken);
    }
}