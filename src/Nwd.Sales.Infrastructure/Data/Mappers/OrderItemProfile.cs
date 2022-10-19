using AutoMapper;

namespace Nwd.Sales.Infrastructure.Data.Mappers
{
    internal class OrderItemProfile : Profile
    {
        public OrderItemProfile()
        {
            CreateMap<Domain.Orders.OrderItem, Entities.OrderItem>();
            CreateMap<Entities.OrderItem, Domain.Orders.OrderItem>();
        }
    }
}
