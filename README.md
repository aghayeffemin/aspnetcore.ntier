# Asp.Net Core Web API N-Tier

.NET Core N-Tier architecture Web Api sample project.

## Setup

- SQLite has been used as database
- You can change connection string from *appsettings.json* within *aspnetcore.ntier.API*
- Apply database migrations to create the tables. From a command line :

Go into aspnetcore.ntier.DAL class library. Please make sure proper versions of[.NET SDK](https://automapper.org/) and [dotnet-ef](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) tool has been installed
```
cd aspnetcore.ntier.DAL
```
Apply database changes.
If you are using SQLite then database file with .db extension should be created inside aspnetcore.ntier.API project
```
dotnet ef --startup-project ../aspnetcore.ntier.API database update --context AspNetCoreNTierDbContext
```
## Layers

- aspnetcore.ntier.API - Presentation Layer *.Net Core Web API project*.
- aspnetcore.ntier.BLL - Business Logic Layer responsible for data exchange between DAL and Presentation Layer.
- aspnetcore.ntier.DAL - Data Access Layer responsible for interacting database. *Generic repositories* have been used.
- aspnetcore.ntier.DTO - Data transfer objects.
- aspnetcore.ntier.Entity - Database entities.
- aspnetcore.ntier.IoC - Responsible for *dependency injection* it has ```DependencyInjection``` class and ```InjectDependencies``` method in it.
- aspnetcore.ntier.Test - Used xUnit and Mock tools.

## Development process
You can follow the steps during development from [here](https://github.com/aghayeffemin/aspnetcore.ntier/commits/master). I hope it will be helpful to you in order to understand the development process.

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## Show your support

Give a ⭐️ if this project helped you!
