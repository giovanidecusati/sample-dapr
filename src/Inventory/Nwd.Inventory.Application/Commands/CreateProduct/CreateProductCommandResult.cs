namespace Nwd.Inventory.Application.Commands.CreateProduct
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
