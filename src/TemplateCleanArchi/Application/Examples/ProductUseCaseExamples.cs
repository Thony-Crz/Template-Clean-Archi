using TemplateCleanArchi.Application.DTOs;
using TemplateCleanArchi.Application.UseCases.product;
using TemplateCleanArchi.Domain.Interfaces;

namespace TemplateCleanArchi.Application.Examples;

/// <summary>
/// Exemple d'utilisation des UseCases avec l'approche DDD
/// Pattern: new ...UseCase(dependencies).Execute(input)
/// </summary>
public class ProductUseCaseExamples
{
    private readonly IProductRepository _productRepository;

    public ProductUseCaseExamples(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    /// <summary>
    /// Exemple: Créer un nouveau produit
    /// </summary>
    public async Task<Guid> CreateNewProduct()
    {
        // Créer le UseCase avec ses dépendances
        var createProductUseCase = new CreateProductUseCase(_productRepository);
        
        // Préparer la requête
        var request = new CreateProductRequest("iPhone 15 Pro", 1299.99m);
        
        // Exécuter le UseCase
        var productId = await createProductUseCase.Execute(request);
        
        return productId;
    }

    /// <summary>
    /// Exemple: Récupérer un produit existant
    /// </summary>
    public async Task GetExistingProduct(Guid productId)
    {
        // Créer le UseCase avec ses dépendances
        var getProductUseCase = new GetProductUseCase(_productRepository);
        
        // Exécuter le UseCase avec l'ID du produit
        var product = await getProductUseCase.Execute(productId);
        
        Console.WriteLine($"Product: {product.Name} - Price: {product.Price}€");
    }

    /// <summary>
    /// Exemple: Workflow complet - Créer et récupérer un produit
    /// </summary>
    public async Task CreateAndRetrieveProduct()
    {
        // Étape 1: Créer un nouveau produit
        var createUseCase = new CreateProductUseCase(_productRepository);
        var createRequest = new CreateProductRequest("MacBook Pro", 2499.99m);
        var productId = await createUseCase.Execute(createRequest);
        
        // Étape 2: Récupérer le produit créé
        var getUseCase = new GetProductUseCase(_productRepository);
        var product = await getUseCase.Execute(productId);
        
        Console.WriteLine($"Created and retrieved: {product.Name}");
    }
}
