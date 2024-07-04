# Express Voitures - Exécuter l'application en local

## Prérequis

- [.NET Core SDK](https://dotnet.microsoft.com/download)

## Installation

1. Clonez le dépôt :

```bash
git clone https://github.com/ClementBartholome/P5-Dotnet
```
2. Accédez au dossier du projet :

```bash
cd <chemin_vers_le_dépôt_cloné>/P5-Dotnet
```

3. Installez les dépendances .NET :

```bash
dotnet restore
```

## Configuration de la base de données

1. Installez [SQL Server](https://www.microsoft.com/fr-fr/sql-server/sql-server-downloads) sur votre machine.

2. Ouvrez SQL Server Management Studio et connectez-vous à votre instance de SQL Server.

3. Créez une nouvelle base de données pour l'application.

4. Dans le répertoire racine du projet, créez un nouveau fichier nommé appsettings.Development.json.

5. Ouvrez le fichier appsettings.Development.json et ajoutez le contenu suivant :

```json
"ConnectionStrings": {
    "DefaultConnection": "Server=(local);Database=your_database_name;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

Remplacez "your_database_name" par le nom de la base de données que vous avez créée précédemment.


## Exécution de l'application

1. Exécutez l'application en mode développement :

```bash
dotnet run
```

2. Ouvrez votre navigateur et accédez à `https://localhost:7260/`.

## Connexion en tant qu'administrateur

1. Une fois que l'application est en cours d'exécution, ouvrez votre navigateur et accédez à https://localhost:7260/Admin.  

2. Vous serez redirigé vers la page de connexion. Utilisez les identifiants suivants pour vous connecter :

        - Nom d'utilisateur : admin@expressvoitures.com
        - Mot de passe : Password123$
  
# Express Voitures - Azure

Le projet dispose d'une version déployée sur Azure, disponible ici : https://webapp-240428223131.azurewebsites.net/ 

La connexion administrateur se fait ici : https://webapp-240428223131.azurewebsites.net/admin

Avec les identifiants suivants : 

    - Nom d'utilisateur : admin@expressvoitures.com
    - Mot de passe : Password123$
