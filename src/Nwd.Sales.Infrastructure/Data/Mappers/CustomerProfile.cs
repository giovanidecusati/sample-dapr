using AutoMapper;

namespace Nwd.Sales.Infrastructure.Data.Mappers
{
    internal class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Domain.Orders.Customer, Entities.Customer>();
            CreateMap<Entities.Customer, Domain.Orders.Customer>();
        }
    }
}
