namespace Nwd.Inventory.Application.Commands.CreateCategory
{
    public class CreateCategoryCommandResult
    {
        public CreateCategoryCommandResult(string categoryId)
        {
            CategoryId = categoryId;
        }

        public string CategoryId { get; private set; }
    }
}
