using AutoMapper;

namespace Nwd.Sales.Infrastructure.Data.Mapper
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Domain.Orders.OrderAgg, Infrastructure.Data.Entities.Order>();
        }
    }
}
