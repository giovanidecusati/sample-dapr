namespace Nwd.Orders.Domain.Entities
{
    public enum OrderStatus : int
    {
        Submitted,
        Processing,
        Completed,
        Error
    }
}