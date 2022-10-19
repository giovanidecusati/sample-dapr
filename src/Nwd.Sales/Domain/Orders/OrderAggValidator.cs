using FluentValidation;

namespace Nwd.Sales.Domain.Orders
{
    public class OrderAggValidator : AbstractValidator<OrderAgg>
    {
        public OrderAggValidator()
        {
            RuleFor(i => i.Items.Count).GreaterThan(0);
        }
    }
}
