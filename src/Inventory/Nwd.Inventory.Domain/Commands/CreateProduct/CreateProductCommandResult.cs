namespace Nwd.Inventory.Domain.Commands.CreateCategory
{
    public class CreateProductCommandResult
    {
        public CreateProductCommandResult(string productId)
        {
            ProductId = productId;
        }

        public string ProductId { get; private set; }
    }
}
