﻿namespace Nwd.Sales.Domain.Orders
{
    public interface IOrderRepository
    {
        Task AddAsync(Order entity);                
    }
}
