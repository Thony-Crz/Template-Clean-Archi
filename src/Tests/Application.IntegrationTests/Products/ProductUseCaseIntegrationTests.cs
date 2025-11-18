using TemplateCleanArchi.Application.DTOs;
using TemplateCleanArchi.Application.Exceptions;
using TemplateCleanArchi.Application.UseCases.product;
using TemplateCleanArchi.Domain.Entities;
using TemplateCleanArchi.Domain.Interfaces;
using Infrastructure.Repositories;

namespace Application.IntegrationTests.Products
{
    /// <summary>
    /// Tests d'intégration pour les cas d'utilisation Product.
    /// Ces tests vérifient l'intégration complète entre les UseCases et le Repository
    /// avec une base de données en mémoire.
    /// </summary>
    [TestFixture]
    public class ProductUseCaseIntegrationTests
    {
        private InMemoryProductRepository _repository;

        [SetUp]
        public void SetUp()
        {
            // Utiliser un repository en mémoire pour les tests d'intégration
            _repository = new InMemoryProductRepository();
        }

        [Test]
        public async Task CreateAndRetrieveProduct_ShouldWorkEndToEnd()
        {
            // Arrange
            var productName = "iPhone 15 Pro";
            var productPrice = 1299.99m;
            var createRequest = new CreateProductRequest(productName, productPrice);

            // Act - Créer un produit
            var createUseCase = new CreateProductUseCase(_repository);
            var productId = await createUseCase.Execute(createRequest);

            // Act - Récupérer le produit créé
            var getUseCase = new GetProductUseCase(_repository);
            var retrievedProduct = await getUseCase.Execute(productId);

            // Assert
            Assert.That(productId, Is.Not.EqualTo(Guid.Empty));
            Assert.That(retrievedProduct, Is.Not.Null);
            Assert.That(retrievedProduct.Id, Is.EqualTo(productId));
            Assert.That(retrievedProduct.Name, Is.EqualTo(productName));
            Assert.That(retrievedProduct.Price, Is.EqualTo(productPrice));
        }

        [Test]
        public async Task CreateMultipleProducts_ShouldStoreAndRetrieveAll()
        {
            // Arrange
            var products = new[]
            {
                new CreateProductRequest("Product 1", 100m),
                new CreateProductRequest("Product 2", 200m),
                new CreateProductRequest("Product 3", 300m)
            };
            var createUseCase = new CreateProductUseCase(_repository);

            // Act - Créer plusieurs produits
            var productIds = new List<Guid>();
            foreach (var request in products)
            {
                var id = await createUseCase.Execute(request);
                productIds.Add(id);
            }

            // Act - Récupérer tous les produits
            var allProducts = await _repository.GetAllAsync();

            // Assert
            Assert.That(productIds.Count, Is.EqualTo(3));
            Assert.That(allProducts.Count(), Is.EqualTo(3));
            
            var productList = allProducts.ToList();
            Assert.That(productList[0].Name, Is.EqualTo("Product 1"));
            Assert.That(productList[1].Name, Is.EqualTo("Product 2"));
            Assert.That(productList[2].Name, Is.EqualTo("Product 3"));
        }

        [Test]
        public void GetNonExistentProduct_ShouldThrowNotFoundException()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            var getUseCase = new GetProductUseCase(_repository);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(
                async () => await getUseCase.Execute(nonExistentId),
                "L'opération devrait lever une NotFoundException pour un produit inexistant"
            );
        }

        [Test]
        public async Task CreateProduct_WithValidData_ShouldGenerateUniqueIds()
        {
            // Arrange
            var request1 = new CreateProductRequest("Product A", 100m);
            var request2 = new CreateProductRequest("Product B", 200m);
            var createUseCase = new CreateProductUseCase(_repository);

            // Act
            var id1 = await createUseCase.Execute(request1);
            var id2 = await createUseCase.Execute(request2);

            // Assert
            Assert.That(id1, Is.Not.EqualTo(id2), "Chaque produit devrait avoir un ID unique");
            Assert.That(id1, Is.Not.EqualTo(Guid.Empty));
            Assert.That(id2, Is.Not.EqualTo(Guid.Empty));
        }
    }

    /// <summary>
    /// Implémentation en mémoire du IProductRepository pour les tests d'intégration.
    /// Cette classe simule une base de données en utilisant une collection en mémoire.
    /// </summary>
    public class InMemoryProductRepository : IProductRepository
    {
        private readonly Dictionary<Guid, Product> _products = new();

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Product>>(_products.Values.ToList());
        }

        public Task<Product?> GetAsync(Guid productId)
        {
            _products.TryGetValue(productId, out var product);
            return Task.FromResult(product);
        }

        public Task AddAsync(Product product)
        {
            _products[product.Id] = product;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Product product)
        {
            if (_products.ContainsKey(product.Id))
            {
                _products[product.Id] = product;
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid productId)
        {
            _products.Remove(productId);
            return Task.CompletedTask;
        }
    }
}
