using TemplateCleanArchi.Domain.Entities;
using TemplateCleanArchi.Domain.Interfaces;

namespace TemplateCleanArchi.Application.UseCases.product;

public class CreateProductHandler(IProductRepository repository)
{
    private readonly IProductRepository _repository = repository;

    public async Task<Guid> Handle(string name, decimal price)
    {
        var product = new Product(name, price);
        await _repository.AddAsync(product);
        return product.Id;
    }
}
