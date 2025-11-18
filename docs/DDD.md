# Approche Domain-Driven Design (DDD)

## Introduction

Ce template utilise l'approche **Domain-Driven Design (DDD)**, une philosophie de conception logicielle qui met l'accent sur la modélisation du domaine métier et la collaboration entre experts techniques et experts métier.

## Principes Fondamentaux

### 1. Le Domaine au Centre

Le **domaine métier** est au cœur de l'architecture. Toutes les décisions de conception sont guidées par la compréhension approfondie des besoins métier.

### 2. Langage Ubiquitaire (Ubiquitous Language)

Un langage commun partagé entre les développeurs et les experts métier. Les termes utilisés dans le code doivent correspondre exactement aux termes utilisés dans les discussions métier.

**Exemple dans ce projet :**
- `Product` - Entité représentant un produit
- `CreateProductUseCase` - Cas d'utilisation pour créer un produit
- `ProductRepository` - Repository pour accéder aux produits

### 3. Architecture en Couches

Le projet est organisé en couches distinctes pour séparer les préoccupations :

```
┌─────────────────────────────────────┐
│         WebApi (Présentation)       │  ← Interface utilisateur / API
├─────────────────────────────────────┤
│         Application (UseCases)      │  ← Logique applicative
├─────────────────────────────────────┤
│         Domain (Entités)            │  ← Logique métier pure
├─────────────────────────────────────┤
│         Infrastructure              │  ← Accès aux données, services externes
└─────────────────────────────────────┘
```

## Structure du Projet

### Domain Layer (Couche Domaine)

**Emplacement :** `src/TemplateCleanArchi/Domain/`

Le domaine contient :
- **Entités** : Objets métier avec identité (`Product`)
- **Interfaces** : Contrats pour les repositories (`IProductRepository`)
- **Value Objects** : Objets immuables sans identité (à ajouter selon les besoins)

**Caractéristiques importantes :**
- ✅ Aucune dépendance vers les autres couches
- ✅ Logique métier pure et testable
- ✅ Entités avec encapsulation forte (propriétés privates)

**Exemple - Product Entity :**
```csharp
public class Product
{
    public Guid Id { get; set; }
    public string Name { get; private set; } = null!;
    public decimal Price { get; private set; }

    private Product() { } // Pour EF Core
    
    public Product(string name, decimal price)
    {
        Id = Guid.NewGuid();
        Name = name;
        Price = price;
    }
}
```

### Application Layer (Couche Application)

**Emplacement :** `src/TemplateCleanArchi/Application/`

La couche application contient :
- **UseCases** : Scénarios d'utilisation métier
- **DTOs** : Objets de transfert de données
- **Interfaces** : Contrats pour les services (`IUseCase`, `IUnitOfWork`)
- **Exceptions** : Exceptions métier personnalisées

**Pattern UseCase :**

Chaque action métier est représentée par un UseCase distinct qui implémente `IUseCase<TInput, TResult>`.

```csharp
public interface IUseCase<in TInput, TResult>
{
    Task<TResult> Execute(TInput input);
}
```

**Exemple - CreateProductUseCase :**
```csharp
public class CreateProductUseCase : IUseCase<CreateProductRequest, Guid>
{
    private readonly IProductRepository _repository;

    public CreateProductUseCase(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Execute(CreateProductRequest request)
    {
        var product = new Product(request.Name, request.Price);
        await _repository.AddAsync(product);
        return product.Id;
    }
}
```

### Infrastructure Layer (Couche Infrastructure)

**Emplacement :** `src/Infrastructure/`

L'infrastructure contient :
- **Repositories** : Implémentations concrètes des repositories
- **DbContext** : Contexte Entity Framework Core
- **Services externes** : Intégrations avec APIs, systèmes de fichiers, etc.

**Caractéristiques :**
- ✅ Implémente les interfaces définies dans le domaine
- ✅ Contient les détails techniques (EF Core, SQL, etc.)
- ✅ Peut être remplacée sans affecter le domaine

### Presentation Layer (WebApi)

**Emplacement :** `WebApi/`

La couche présentation contient :
- **Controllers** : Points d'entrée API
- **Configuration** : Injection de dépendances, middleware
- **Models de requête/réponse** : Spécifiques à l'API

## Avantages de l'Approche DDD

### 1. Séparation des Préoccupations

Chaque couche a une responsabilité claire :
- Le **domaine** gère la logique métier
- L'**application** orchestre les cas d'utilisation
- L'**infrastructure** gère les détails techniques
- La **présentation** gère l'interface utilisateur

### 2. Testabilité

- Les entités du domaine sont **pures** et facilement testables
- Les UseCases peuvent être testés avec des **mocks** des repositories
- Les tests d'intégration peuvent utiliser des **repositories en mémoire**

### 3. Maintenabilité

- Le code métier est **isolé** des détails techniques
- Les changements dans l'infrastructure n'affectent pas le domaine
- Facile d'ajouter de nouveaux cas d'utilisation

### 4. Évolutivité

- Nouvelle fonctionnalité = Nouveau UseCase
- Nouveau système de stockage = Nouvelle implémentation du repository
- Nouvelle API = Nouveau controller

## Règles d'Architecture

### Règle de Dépendance

Les dépendances vont toujours **de l'extérieur vers l'intérieur** :

```
WebApi → Application → Domain
  ↓           ↓
Infrastructure
```

- ✅ L'application peut dépendre du domaine
- ✅ L'infrastructure peut dépendre du domaine et de l'application
- ✅ La présentation peut dépendre de toutes les couches
- ❌ Le domaine NE DOIT PAS dépendre de l'application ou de l'infrastructure

### Inversion de Dépendance

Les interfaces sont définies dans le domaine, mais implémentées dans l'infrastructure :

```csharp
// Domain/Interfaces/IProductRepository.cs
public interface IProductRepository { }

// Infrastructure/Repositories/ProductRepository.cs
public class ProductRepository : IProductRepository { }
```

## Exemples d'Utilisation

### Créer un Nouveau UseCase

1. **Définir le DTO de requête** (Application/DTOs)
```csharp
public record UpdateProductRequest(Guid Id, string Name, decimal Price);
```

2. **Créer le UseCase** (Application/UseCases)
```csharp
public class UpdateProductUseCase : IUseCase<UpdateProductRequest, bool>
{
    private readonly IProductRepository _repository;

    public UpdateProductUseCase(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Execute(UpdateProductRequest request)
    {
        var product = await _repository.GetAsync(request.Id);
        if (product == null) return false;
        
        // Mise à jour via méthode du domaine
        product.UpdateDetails(request.Name, request.Price);
        await _repository.UpdateAsync(product);
        return true;
    }
}
```

3. **Utiliser dans un Controller** (WebApi)
```csharp
[HttpPut("{id}")]
public async Task<IActionResult> Update(Guid id, UpdateProductRequest request)
{
    var useCase = new UpdateProductUseCase(_productRepository);
    var result = await useCase.Execute(request);
    return result ? Ok() : NotFound();
}
```

## Comparaison : DDD vs CQRS

Ce template utilise **DDD avec UseCases** plutôt que **CQRS** (Command Query Responsibility Segregation).

| Aspect | DDD avec UseCases | CQRS |
|--------|-------------------|------|
| **Séparation** | Par cas d'utilisation métier | Par commande/requête |
| **Complexité** | Plus simple | Plus complexe |
| **Use Cases** | `CreateProductUseCase`, `GetProductUseCase` | `CreateProductCommand`, `GetProductQuery` |
| **Handlers** | Un par UseCase | CommandHandler et QueryHandler séparés |
| **Adapté pour** | Projets de taille petite à moyenne | Projets complexes avec lecture/écriture intensive |

## Ressources

- [Domain-Driven Design par Eric Evans](https://www.domainlanguage.com/ddd/)
- [Clean Architecture par Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Documentation Microsoft sur DDD](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/)

## Conclusion

L'approche DDD dans ce template vous permet de :
- 🎯 Modéliser le métier de façon claire et précise
- 🧪 Écrire du code hautement testable
- 🔧 Maintenir facilement votre code
- 📈 Faire évoluer votre application sereinement

Le code métier reste **pur** et **indépendant** des détails techniques, ce qui facilite grandement la compréhension, les tests et la maintenance de l'application.
