using TemplateCleanArchi.Application.Exceptions;
using TemplateCleanArchi.Application.Interfaces;
using TemplateCleanArchi.Domain.Entities;
using TemplateCleanArchi.Domain.Interfaces;

namespace TemplateCleanArchi.Application.UseCases.product
{
    public class GetProductUseCase(IProductRepository repository) : IUseCase<Guid, Product>
    {
        private readonly IProductRepository _repository = repository;

        public async Task<Product> Execute(Guid productId)
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
