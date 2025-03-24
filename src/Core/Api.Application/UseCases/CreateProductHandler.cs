using Core.Domain.Entities;
using Core.Domain.Interfaces;

namespace Core.Application.UseCases;

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
