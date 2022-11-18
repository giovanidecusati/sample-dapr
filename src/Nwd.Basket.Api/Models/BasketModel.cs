namespace Nwd.Basket.Api.Models
{
    public class BasketModel
    {
        public string Id { get; set; }

        public string CustomerId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public List<BasketItemModel> Items { get; set; }
    }
}