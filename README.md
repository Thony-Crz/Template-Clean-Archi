# Template Clean Architecture .NET 8.0

## Description

Ce projet est une application .NET 8.0 utilisant C# 12.0. Il est structuré en plusieurs couches, notamment l'infrastructure, le domaine et les tests. L'objectif principal de ce projet est de fournir une architecture modulaire et testable pour une application de gestion de [votre domaine].

## Structure du Projet

- **src/Infrastructure** : Contient les implémentations des dépôts et le contexte de la base de données.
  - `AppDbContext.cs` : Classe représentant le contexte de la base de données.
  - `Repositories/[NomDuRepository].cs` : Implémentation du dépôt pour [votre entité].

- **src/Core** : Contient les entités du domaine et les interfaces. Les entités du domaine doivent être pures, c'est-à-dire qu'elles ne doivent pas avoir d'effets de bord et doivent être déterministes.
  - `Entities/[NomDeLEntité].cs` : Classe représentant [votre entité].
  - `Interfaces/I[NomDuRepository].cs` : Interface pour le dépôt de [votre entité].

- **src/Tests** : Contient les projets de tests unitaires et d'intégration.
  - `[NomDuProjet].UnitTests` : Tests unitaires pour l'application.
  - `[NomDuProjet].IntegrationTests` : Tests d'intégration pour l'application.

## Prérequis

- .NET 8.0 SDK
- Visual Studio 2022

## Installation

1. Clonez le dépôt :

`git clone ...`

2. Accédez au répertoire du projet

3. Restaurez les packages NuGet :

`dotnet restore`

## Ce qu'il reste à faire

- Configurer l'injection de dépendance pour les services et les dépôts.

## Licence

Ce projet est sous licence MIT. Voir le fichier [LICENSE](LICENSE) pour plus de détails.