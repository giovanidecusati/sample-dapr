using FluentValidation;

namespace Nwd.Sales.Domain.Orders
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(i => i.Items.Count).GreaterThan(0);
        }
    }
}
