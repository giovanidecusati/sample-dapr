using AutoMapper;

namespace Nwd.Sales.Infrastructure.Data.Mappers
{
    internal class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Domain.Orders.Order, Entities.Order>();
            CreateMap<Entities.Order, Domain.Orders.Order>();

            CreateMap<Entities.Order, Application.Queries.GetOrder.GetSingleOrderQueryResult>();
        }
    }
}
