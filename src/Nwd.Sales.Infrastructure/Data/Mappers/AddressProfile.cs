using AutoMapper;

namespace Nwd.Sales.Infrastructure.Data.Mappers
{
    internal class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<Domain.Orders.Address, Entities.Address>();
            CreateMap<Entities.Address, Domain.Orders.Address>();

            CreateMap<Entities.Address, Application.Queries.GetOrder.Address>();
        }
    }
}
