namespace Nwd.Basket.Api.Models
{
    public class BasketItemModel
    {
        public int Sequence { get; set; }

        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public string CategoryName { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Total { get; set; }
    }
}