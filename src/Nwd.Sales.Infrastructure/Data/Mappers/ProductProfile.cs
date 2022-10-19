using AutoMapper;

namespace Nwd.Sales.Infrastructure.Data.Mappers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Domain.Orders.Product, Entities.Product>();
            CreateMap<Entities.Product, Domain.Orders.Product>();
        }
    }
}
