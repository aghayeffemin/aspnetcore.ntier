# Asp.Net Core Web API N-Tier

.NET Core N-Tier architecture Web Api sample project.

## Setup

- SQLite has been used as database
- You can change connection string from *appsettings.json* within *aspnetcore.ntier.API*
- Apply database migrations to create the tables. From a command line :

Go into aspnetcore.ntier.DAL class library
```
cd aspnetcore.ntier.DAL
```
Add migrations
```
dotnet ef --startup-project ../aspnetcore.ntier.API migrations add InitialMigration --context AspNetCoreNTierDbContext
```
Apply database changes. If you are using SQLite then database file with .db extension should be created inside aspnetcore.ntier.API project
```
dotnet ef --startup-project ../aspnetcore.ntier.API database update InitialMigration --context AspNetCoreNTierDbContext
```
## Layers

- aspnetcore.ntier.API - Presentation Layer *.Net Core Web API project*.
- aspnetcore.ntier.BLL - Business Logic Layer responsible for data exchange between DAL and Presentation Layer.
- aspnetcore.ntier.DAL - Data Access Layer responsible for interacting database. *Generic repositories* have been used.
- aspnetcore.ntier.DTO - Data transfer objects.
- aspnetcore.ntier.Entity - Database entities.
- aspnetcore.ntier.IoC - Responsible for *dependency injection* it has ```DependencyInjection``` class and ```InjectDependencies``` method in it.
- aspnetcore.ntier.Test - Used xUnit and Mock tools.
- aspnetcore.ntier.Utility - Has *AutoMapperProfiles* (You can get detailed information about *Automapper* from [here](https://automapper.org/)) class in it.

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## Show your support

Give a ⭐️ if this project helped you!
