# Test-Driven Development (TDD)

## Introduction

Le **Test-Driven Development (TDD)** est une pratique de développement logiciel qui consiste à écrire les tests **avant** d'écrire le code de production. Cette approche garantit que le code est testable dès sa conception et qu'il répond exactement aux exigences.

## Le Cycle TDD : Red-Green-Refactor

Le TDD suit un cycle en trois étapes :

```
🔴 RED → 🟢 GREEN → 🔵 REFACTOR → 🔴 RED → ...
```

### 1. 🔴 RED (Rouge) - Écrire un test qui échoue

Écrivez un test pour une fonctionnalité qui n'existe pas encore. Le test doit échouer car le code n'est pas encore implémenté.

### 2. 🟢 GREEN (Vert) - Écrire le code minimal pour faire passer le test

Écrivez juste assez de code pour faire passer le test. Ne vous préoccupez pas de la perfection à ce stade.

### 3. 🔵 REFACTOR (Refactorisation) - Améliorer le code

Améliorez le code tout en gardant les tests verts. Éliminez la duplication, améliorez la lisibilité et la structure.

## Types de Tests dans ce Template

### 1. Tests Unitaires

**Emplacement :** `src/Tests/Application.UnitTests/`

Les tests unitaires testent une unité de code isolée (une classe, une méthode) en isolation complète des dépendances externes.

**Caractéristiques :**
- ✅ Rapides à exécuter (millisecondes)
- ✅ Utilisent des **mocks** pour simuler les dépendances
- ✅ Testent la logique métier pure
- ✅ Indépendants les uns des autres

**Exemple - Test Unitaire d'un UseCase :**
```csharp
[TestFixture]
public class CreateProductUseCaseTests
{
    private Mock<IProductRepository> _productRepositoryMock;
    private CreateProductUseCase _useCase;

    [SetUp]
    public void SetUp()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _useCase = new CreateProductUseCase(_productRepositoryMock.Object);
    }

    [Test]
    public async Task Execute_ShouldAddProductAndReturnId()
    {
        // Arrange
        var request = new CreateProductRequest("Test Product", 100m);
        var productId = Guid.NewGuid();

        _productRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Product>()))
            .Callback<Product>(product => product.Id = productId)
            .Returns(Task.CompletedTask);

        // Act
        var result = await _useCase.Execute(request);

        // Assert
        Assert.That(result, Is.EqualTo(productId));
        _productRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Once);
    }
}
```

**Quand utiliser :**
- Pour tester la logique métier des UseCases
- Pour tester les entités du domaine
- Pour tester les validations

### 2. Tests d'Intégration

**Emplacement :** `src/Tests/Application.IntegrationTests/`

Les tests d'intégration testent l'intégration entre plusieurs composants (UseCases + Repository, par exemple).

**Caractéristiques :**
- ✅ Plus lents que les tests unitaires
- ✅ Utilisent des **implémentations réelles** ou en mémoire
- ✅ Testent les interactions entre composants
- ✅ Plus proches de l'utilisation réelle

**Exemple - Test d'Intégration :**
```csharp
[TestFixture]
public class ProductUseCaseIntegrationTests
{
    private InMemoryProductRepository _repository;

    [SetUp]
    public void SetUp()
    {
        _repository = new InMemoryProductRepository();
    }

    [Test]
    public async Task CreateAndRetrieveProduct_ShouldWorkEndToEnd()
    {
        // Arrange
        var createRequest = new CreateProductRequest("iPhone 15 Pro", 1299.99m);

        // Act - Créer un produit
        var createUseCase = new CreateProductUseCase(_repository);
        var productId = await createUseCase.Execute(createRequest);

        // Act - Récupérer le produit créé
        var getUseCase = new GetProductUseCase(_repository);
        var retrievedProduct = await getUseCase.Execute(productId);

        // Assert
        Assert.That(retrievedProduct.Name, Is.EqualTo("iPhone 15 Pro"));
        Assert.That(retrievedProduct.Price, Is.EqualTo(1299.99m));
    }
}
```

**Quand utiliser :**
- Pour tester le flux complet d'un cas d'utilisation
- Pour vérifier l'intégration avec la base de données
- Pour tester les transactions et la persistance

## Pratique TDD : Exemple Pas à Pas

Imaginons que nous voulons ajouter une fonctionnalité pour mettre à jour le prix d'un produit.

### Étape 1 : 🔴 RED - Écrire le test qui échoue

```csharp
[TestFixture]
public class UpdateProductPriceUseCaseTests
{
    private Mock<IProductRepository> _repositoryMock;
    private UpdateProductPriceUseCase _useCase;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _useCase = new UpdateProductPriceUseCase(_repositoryMock.Object);
    }

    [Test]
    public async Task Execute_ShouldUpdateProductPrice()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product("Test Product", 100m) { Id = productId };
        var newPrice = 150m;

        _repositoryMock
            .Setup(repo => repo.GetAsync(productId))
            .ReturnsAsync(product);

        // Act
        var request = new UpdateProductPriceRequest(productId, newPrice);
        await _useCase.Execute(request);

        // Assert
        Assert.That(product.Price, Is.EqualTo(150m));
        _repositoryMock.Verify(repo => repo.UpdateAsync(product), Times.Once);
    }
}
```

À ce stade, le test **échoue** car :
- `UpdateProductPriceUseCase` n'existe pas
- `UpdateProductPriceRequest` n'existe pas
- La méthode `UpdatePrice` sur `Product` n'existe pas

### Étape 2 : 🟢 GREEN - Implémenter le code minimal

**Créer le DTO de requête :**
```csharp
// Application/DTOs/UpdateProductPriceRequest.cs
public record UpdateProductPriceRequest(Guid ProductId, decimal NewPrice);
```

**Ajouter la méthode au domaine :**
```csharp
// Domain/Entities/Product.cs
public class Product
{
    // ... propriétés existantes ...

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new ArgumentException("Le prix doit être positif", nameof(newPrice));
        
        Price = newPrice;
    }
}
```

**Créer le UseCase :**
```csharp
// Application/UseCases/product/UpdateProductPriceUseCase.cs
public class UpdateProductPriceUseCase : IUseCase<UpdateProductPriceRequest, bool>
{
    private readonly IProductRepository _repository;

    public UpdateProductPriceUseCase(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Execute(UpdateProductPriceRequest request)
    {
        var product = await _repository.GetAsync(request.ProductId);
        if (product == null) return false;

        product.UpdatePrice(request.NewPrice);
        await _repository.UpdateAsync(product);
        return true;
    }
}
```

**Exécuter les tests → ✅ Vert !**

### Étape 3 : 🔵 REFACTOR - Améliorer le code

Maintenant que le test passe, nous pouvons améliorer le code :

**Ajouter un test pour le cas d'erreur :**
```csharp
[Test]
public void UpdatePrice_WithNegativePrice_ShouldThrowException()
{
    // Arrange
    var product = new Product("Test", 100m);

    // Act & Assert
    Assert.Throws<ArgumentException>(() => product.UpdatePrice(-50m));
}
```

**Ajouter un test pour produit inexistant :**
```csharp
[Test]
public async Task Execute_WithNonExistentProduct_ShouldReturnFalse()
{
    // Arrange
    var productId = Guid.NewGuid();
    _repositoryMock
        .Setup(repo => repo.GetAsync(productId))
        .ReturnsAsync((Product?)null);

    // Act
    var request = new UpdateProductPriceRequest(productId, 150m);
    var result = await _useCase.Execute(request);

    // Assert
    Assert.That(result, Is.False);
}
```

## Structure de Test : AAA Pattern

Tous les tests suivent le pattern **AAA** (Arrange-Act-Assert) :

```csharp
[Test]
public async Task NomDescriptifDuTest()
{
    // Arrange - Préparer les données et les mocks
    var request = new CreateProductRequest("Product", 100m);
    // ... configuration des mocks ...

    // Act - Exécuter le code à tester
    var result = await _useCase.Execute(request);

    // Assert - Vérifier les résultats
    Assert.That(result, Is.EqualTo(expectedValue));
}
```

### Arrange (Préparer)
- Créer les objets nécessaires
- Configurer les mocks
- Préparer les données de test

### Act (Agir)
- Appeler la méthode à tester
- Une seule ligne dans la plupart des cas

### Assert (Vérifier)
- Vérifier que le résultat est correct
- Vérifier que les mocks ont été appelés correctement

## Conventions de Nommage des Tests

### Format : `MethodeName_StateUnderTest_ExpectedBehavior`

**Exemples :**
- ✅ `Execute_ShouldAddProductAndReturnId`
- ✅ `Execute_WhenProductExists_ShouldReturnProduct`
- ✅ `Execute_WhenProductDoesNotExist_ShouldThrowNotFoundException`
- ✅ `UpdatePrice_WithNegativePrice_ShouldThrowException`

### Noms Descriptifs

Les noms de tests doivent être suffisamment descriptifs pour comprendre :
1. Ce qui est testé
2. Dans quelles conditions
3. Quel est le comportement attendu

## Outils de Test

### NUnit

Framework de test utilisé dans ce template.

**Attributs principaux :**
- `[TestFixture]` - Marque une classe de tests
- `[Test]` - Marque une méthode de test
- `[SetUp]` - Exécuté avant chaque test
- `[TearDown]` - Exécuté après chaque test
- `[OneTimeSetUp]` - Exécuté une fois avant tous les tests
- `[OneTimeTearDown]` - Exécuté une fois après tous les tests

### Moq

Bibliothèque de mocking pour créer des objets simulés.

**Exemples d'utilisation :**
```csharp
// Créer un mock
var mock = new Mock<IProductRepository>();

// Configurer le comportement
mock.Setup(r => r.GetAsync(It.IsAny<Guid>()))
    .ReturnsAsync(new Product("Test", 100m));

// Vérifier les appels
mock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
mock.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Never);
```

## Commandes Utiles

### Exécuter tous les tests
```bash
dotnet test
```

### Exécuter les tests d'un projet spécifique
```bash
dotnet test src/Tests/Application.UnitTests/
dotnet test src/Tests/Application.IntegrationTests/
```

### Exécuter avec couverture de code
```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Exécuter en mode watch (re-exécute automatiquement)
```bash
dotnet watch test
```

## Avantages du TDD

### 1. 🎯 Code Focalisé sur les Besoins

Le code fait exactement ce dont on a besoin, rien de plus, rien de moins.

### 2. 🧪 Couverture de Tests Élevée

Tous les chemins du code sont testés car les tests sont écrits avant le code.

### 3. 🔧 Code Plus Maintenable

Les tests servent de documentation vivante et permettent de refactoriser en confiance.

### 4. 🐛 Détection Précoce des Bugs

Les problèmes sont détectés dès l'écriture du code, pas en production.

### 5. 💡 Meilleure Conception

Le TDD force à penser à l'API et à l'utilisation avant l'implémentation, conduisant à un meilleur design.

## Bonnes Pratiques

### ✅ DO (À Faire)

- **Écrire les tests d'abord** - C'est l'essence du TDD
- **Un concept par test** - Chaque test vérifie une seule chose
- **Tests indépendants** - Les tests ne doivent pas dépendre les uns des autres
- **Tests rapides** - Les tests unitaires doivent s'exécuter en millisecondes
- **Noms descriptifs** - Le nom du test explique ce qu'il teste
- **Refactoriser régulièrement** - Gardez le code propre

### ❌ DON'T (À Éviter)

- **Ne pas tester les détails d'implémentation** - Testez le comportement, pas l'implémentation
- **Ne pas avoir de logique dans les tests** - Les tests doivent être simples
- **Ne pas ignorer les tests qui échouent** - Corrigez-les immédiatement
- **Ne pas écrire des tests trop complexes** - Si le test est complexe, le code l'est aussi
- **Ne pas dupliquer le code** - Utilisez `[SetUp]` pour la configuration commune

## Exemple Complet de Workflow TDD

### Fonctionnalité : Supprimer un produit

**1. Écrire le test (RED)** 🔴
```csharp
[Test]
public async Task Execute_ShouldDeleteProduct()
{
    // Arrange
    var productId = Guid.NewGuid();
    _repositoryMock.Setup(r => r.DeleteAsync(productId))
        .Returns(Task.CompletedTask);

    // Act
    var useCase = new DeleteProductUseCase(_repositoryMock.Object);
    await useCase.Execute(productId);

    // Assert
    _repositoryMock.Verify(r => r.DeleteAsync(productId), Times.Once);
}
```

**2. Implémenter le code (GREEN)** 🟢
```csharp
public class DeleteProductUseCase : IUseCase<Guid, Task>
{
    private readonly IProductRepository _repository;

    public DeleteProductUseCase(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task Execute(Guid productId)
    {
        await _repository.DeleteAsync(productId);
    }
}
```

**3. Refactoriser (REFACTOR)** 🔵
```csharp
// Ajouter validation
public async Task Execute(Guid productId)
{
    if (productId == Guid.Empty)
        throw new ArgumentException("L'ID du produit est requis", nameof(productId));

    await _repository.DeleteAsync(productId);
}

// Ajouter test correspondant
[Test]
public void Execute_WithEmptyGuid_ShouldThrowArgumentException()
{
    var useCase = new DeleteProductUseCase(_repositoryMock.Object);
    Assert.ThrowsAsync<ArgumentException>(() => useCase.Execute(Guid.Empty));
}
```

## Ressources

- [Test Driven Development par Kent Beck](https://www.amazon.com/Test-Driven-Development-Kent-Beck/dp/0321146530)
- [NUnit Documentation](https://nunit.org/)
- [Moq Documentation](https://github.com/moq/moq4)
- [Clean Code par Robert C. Martin](https://www.amazon.com/Clean-Code-Handbook-Software-Craftsmanship/dp/0132350882)

## Conclusion

Le TDD est plus qu'une technique de test - c'est une **discipline de conception**. En écrivant les tests d'abord :
- Vous concevez de meilleures APIs
- Vous écrivez du code plus maintenable
- Vous avez confiance en vos refactorisations
- Vous produisez du code de meilleure qualité

N'oubliez pas : **Red → Green → Refactor** ! 🔴🟢🔵
