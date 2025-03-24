# Template Clean Architecture .NET 8.0

## Description

Ce projet est une application .NET 8.0 utilisant C# 12.0. Il est structur� en plusieurs couches, notamment l'infrastructure, le domaine et les tests. L'objectif principal de ce projet est de fournir une architecture modulaire et testable pour une application de gestion de [votre domaine].

## Structure du Projet

- **src/Infrastructure** : Contient les impl�mentations des d�p�ts et le contexte de la base de donn�es.
  - `AppDbContext.cs` : Classe repr�sentant le contexte de la base de donn�es.
  - `Repositories/[NomDuRepository].cs` : Impl�mentation du d�p�t pour [votre entit�].

- **src/Core** : Contient les entit�s du domaine et les interfaces. Les entit�s du domaine doivent �tre pures, c'est-�-dire qu'elles ne doivent pas avoir d'effets de bord et doivent �tre d�terministes.
  - `Entities/[NomDeLEntit�].cs` : Classe repr�sentant [votre entit�].
  - `Interfaces/I[NomDuRepository].cs` : Interface pour le d�p�t de [votre entit�].

- **src/Tests** : Contient les projets de tests unitaires et d'int�gration.
  - `[NomDuProjet].UnitTests` : Tests unitaires pour l'application.
  - `[NomDuProjet].IntegrationTests` : Tests d'int�gration pour l'application.

## Pr�requis

- .NET 8.0 SDK
- Visual Studio 2022

## Installation

1. Clonez le d�p�t :

`git clone ...`

2. Acc�dez au r�pertoire du projet

3. Restaurez les packages NuGet :

`dotnet restore`

## Ce qu'il reste � faire

- Configurer l'injection de d�pendance pour les services et les d�p�ts.

## Licence

Ce projet est sous licence MIT. Voir le fichier [LICENSE](LICENSE) pour plus de d�tails.