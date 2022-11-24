using MediatR;

namespace Nwd.Orders.Application.Services.Inventory
{
    public class UpdateInventoryModel
    {
        public string ProductId { get; set; }
        public int Units { get; set; }
    }
}
