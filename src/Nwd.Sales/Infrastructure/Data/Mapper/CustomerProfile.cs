using AutoMapper;

namespace Nwd.Sales.Infrastructure.Data.Mapper
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Entities.Customer, Domain.Orders.Customer>();
        }
    }
}
