# Template Clean Architecture .NET 8.0

## Description

Ce template est une application .NET 8.0 utilisant C# 12.0. Il est structuré en plusieurs couches, notamment l'infrastructure, le domaine et les tests. L'objectif principal de ce template est de fournir une architecture modulaire et testable pour une application de gestion utilisant l'architecture Clean Architecture.

## Installation du Template

Pour installer ce template localement depuis le dépôt:

```bash
dotnet new install .
```

Ou pour installer depuis NuGet (une fois publié):

```bash
dotnet new install CleanArchitecture.Template
```

## Utilisation du Template

Pour créer un nouveau projet à partir de ce template:

```bash
dotnet new clean-arch -n MonProjet
```

Cela créera un nouveau projet avec la structure suivante:
- `MonProjet.Domain` - Contient les entités du domaine et les interfaces
- `MonProjet.Application` - Contient les cas d'utilisation et la logique métier
- `Infrastructure` - Contient les implémentations des dépôts et le contexte de la base de données
- `WebApi` - API Web ASP.NET Core
- `Application.UnitTests` - Tests unitaires

Tous les namespaces seront automatiquement configurés avec le nom de votre projet (ex: `MonProjet.Domain.Entities`, `MonProjet.Application.UseCases`, etc.).

## Structure du Projet

- **src/TemplateCleanArchi/Domain** : Contient les entités du domaine et les interfaces. Les entités du domaine doivent être pures, c'est-à-dire qu'elles ne doivent pas avoir d'effets de bord et doivent être déterministes.
  - `Entities/Product.cs` : Exemple d'entité du domaine.
  - `Interfaces/IProductRepository.cs` : Interface pour le dépôt de produits.

- **src/TemplateCleanArchi/Application** : Contient les cas d'utilisation et la logique métier.
  - `UseCases/product/` : Use Cases pour les opérations sur les produits (approche DDD avec pattern `...UseCase(...).Execute()`).
  - `DTOs/` : Objets de transfert de données.
  - `Exceptions/` : Exceptions personnalisées.

- **src/Infrastructure** : Contient les implémentations des dépôts et le contexte de la base de données.
  - `AppDbContext.cs` : Classe représentant le contexte de la base de données.
  - `Repositories/ProductRepository.cs` : Implémentation du dépôt pour les produits.

- **WebApi** : API Web ASP.NET Core avec injection de dépendance configurée.

- **src/Tests** : Contient les projets de tests unitaires.
  - `Application.UnitTests` : Tests unitaires pour la couche application.

## Prérequis

- .NET 8.0 SDK
- Visual Studio 2022 ou VS Code

## Approche Use Case (DDD)

Ce template utilise une approche orientée cas d'utilisation (Use Case) plutôt que CQRS. Chaque action métier est représentée par un UseCase qui implémente le pattern `...UseCase(...).Execute()`.

### Exemple

```csharp
// Créer un produit
var createProductUseCase = new CreateProductUseCase(productRepository);
var request = new CreateProductRequest("iPhone 15", 999.99m);
var productId = await createProductUseCase.Execute(request);

// Récupérer un produit
var getProductUseCase = new GetProductUseCase(productRepository);
var product = await getProductUseCase.Execute(productId);
```

Voir [UseCases/README.md](src/TemplateCleanArchi/Application/UseCases/README.md) pour plus de détails.

## Installation (pour développement du template)

1. Clonez le dépôt :

```bash
git clone https://github.com/Thony-Crz/Template-Clean-Archi.git
```

2. Accédez au répertoire du projet

3. Restaurez les packages NuGet :

```bash
dotnet restore
```

4. Compilez le projet :

```bash
dotnet build
```

## Désinstallation du Template

Pour désinstaller le template:

```bash
dotnet new uninstall CleanArchitecture.Template
```

Ou si installé depuis un dossier local:

```bash
dotnet new uninstall /chemin/vers/le/template
```

## Ce qu'il reste à faire

- Configurer l'injection de dépendance pour les services et les dépôts.
- Implémenter les méthodes du repository.
- Ajouter Entity Framework Core pour la persistance des données.

## Licence

Ce projet est sous licence MIT. Voir le fichier [LICENSE](LICENSE) pour plus de détails.
