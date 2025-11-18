using TemplateCleanArchi.Application.Exceptions;
using TemplateCleanArchi.Domain.Entities;
using TemplateCleanArchi.Domain.Interfaces;

namespace TemplateCleanArchi.Application.UseCases.product
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
