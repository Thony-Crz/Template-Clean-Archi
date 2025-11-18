# UseCases

Ce dossier contient les cas d'utilisation (Use Cases) de l'application, suivant une approche DDD (Domain-Driven Design).

## Approche Use Case

Contrairement à l'approche CQRS qui sépare les GET et les Commands, nous utilisons ici une approche orientée cas d'utilisation où chaque UseCase représente une action métier spécifique.

## Pattern

Tous les UseCases suivent le pattern : `...UseCase(...).Execute()`

### Exemple d'utilisation

```csharp
// Injection de dépendance
var productRepository = serviceProvider.GetRequiredService<IProductRepository>();

// Créer un produit
var createProductUseCase = new CreateProductUseCase(productRepository);
var request = new CreateProductRequest("iPhone 15", 999.99m);
var productId = await createProductUseCase.Execute(request);

// Récupérer un produit
var getProductUseCase = new GetProductUseCase(productRepository);
var product = await getProductUseCase.Execute(productId);
```

## Interface de base

Les UseCases implémentent l'interface `IUseCase<TInput, TResult>` qui définit la méthode `Execute()`.

```csharp
public interface IUseCase<in TInput, TResult>
{
    Task<TResult> Execute(TInput input);
}
```

## Structure

Chaque UseCase :
- Est une classe indépendante avec ses propres dépendances
- Implémente l'interface `IUseCase<TInput, TResult>`
- Contient une méthode `Execute()` qui encapsule la logique métier
- Est testable unitairement

## Exemples

### GetProductUseCase

Récupère un produit par son ID.

```csharp
var useCase = new GetProductUseCase(productRepository);
var product = await useCase.Execute(productId);
```

### CreateProductUseCase

Crée un nouveau produit.

```csharp
var useCase = new CreateProductUseCase(productRepository);
var request = new CreateProductRequest("Product Name", 99.99m);
var productId = await useCase.Execute(request);
```
