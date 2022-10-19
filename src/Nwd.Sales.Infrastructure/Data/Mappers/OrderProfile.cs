using AutoMapper;

namespace Nwd.Sales.Infrastructure.Data.Mappers
{
    internal class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Domain.Orders.OrderAgg, Entities.Order>();
            CreateMap<Entities.Order, Domain.Orders.OrderAgg>();
        }
    }
}
