namespace Nwd.Inventory.Domain.Entities
{
    public class Category : BaseEntity
    {
        public Category(string name) : base()
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}
