using Core.Application.Exceptions;
using Core.Domain.Entities;
using Core.Domain.Interfaces;

namespace Core.Application.UseCases.product
{
    public class GetProductHandler(IProductRepository repository)
    {
        private readonly IProductRepository _repository = repository;

        public async Task<Product> Handle(Guid productId)
        {
            var product = await _repository.GetAsync(productId);
            if (product == null)
            {
                throw new NotFoundException($"Product with ID {productId} not found.");
            }
            return product;
        }
    }
}
