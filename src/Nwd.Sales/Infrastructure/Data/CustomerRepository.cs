﻿using AutoMapper;
using Microsoft.Azure.Cosmos;
using Nwd.Sales.Domain.Orders;

namespace Nwd.Sales.Infrastructure.Data
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly Container _container;
        private readonly IMapper _mapper;

        public CustomerRepository(Container container, IMapper mapper)
        {
            _container = container;
            _mapper = mapper;
        }

        public async Task<Customer> GetByIdAsync(Guid customerId)
        {
            var response = await _container.ReadItemAsync<Entities.Customer>(customerId.ToString(), new PartitionKey(customerId.ToString()));
            return _mapper.Map<Customer>(response.Resource);
            //try
            //{
            
            //}
            //catch (CosmosException) //For handling item not found and other exceptions
            //{
            //    return null;
            //}
        }
    }
}
