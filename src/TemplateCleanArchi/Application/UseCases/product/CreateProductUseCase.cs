using TemplateCleanArchi.Application.DTOs;
using TemplateCleanArchi.Application.Interfaces;
using TemplateCleanArchi.Domain.Entities;
using TemplateCleanArchi.Domain.Interfaces;

namespace TemplateCleanArchi.Application.UseCases.product;

public class CreateProductUseCase(IProductRepository repository) : IUseCase<CreateProductRequest, Guid>
{
    private readonly IProductRepository _repository = repository;

    public async Task<Guid> Execute(CreateProductRequest request)
    {
        var product = new Product(request.Name, request.Price);
        await _repository.AddAsync(product);
        return product.Id;
    }
}
