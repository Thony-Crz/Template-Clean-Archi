﻿using Core.Domain.Entities;

namespace Core.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetAsync(Guid productId);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Guid productId);
    }
}
