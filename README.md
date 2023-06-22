# Asp.Net Core Web API N-Tier

.NET Core N-Tier architecture Web Api sample project.

## Give a Star ⭐️

If this repository helped you please consider giving a star! Thanks!

## Setup

- SQLite has been used as database
- You can change connection string from *appsettings.json* within *aspnetcore.ntier.API*
- Apply database migrations to create the tables. From a command line :

Go into aspnetcore.ntier.DAL class library. Please make sure proper versions of [.NET SDK](https://automapper.org/) and [dotnet-ef](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) tool has been installed
```
cd aspnetcore.ntier.DAL
```
Apply database changes.
If you are using SQLite then database file with .db extension should be created inside aspnetcore.ntier.API project
```
dotnet ef --startup-project ../aspnetcore.ntier.API database update --context AspNetCoreNTierDbContext
```
## Authentication

- Authentication has been added for version 2 endpoints.
- Non auth endpoints require JWT token to be provided otherwise 401 response will be returned
- So you have to login or register first in order to get the JWT token then add it to the header while sending a request. It can be done by:
  - Clicking lock icon next to the particular endpoint and pasting token in the textbox on swagger page
  - Adding token for the ```Authorization``` header of the request
- You can disable authentication for endpoints by removing ```Authorize``` attribute from the particular controller

## Versioning

- URL versioning has been implemented and currently there are 2 versions.
- Versions can be changed via Swagger page or by providing version number in the URL.

## Logging

- Structured logging using [Serilog](https://serilog.net/) has been implemented.
- Currently logs are being added to file and console. 
- You can change or find the settings in *appsettings.json*.
- In *UserService* there examples of formatting the log message or using different levels of logging
- Logging will be improved over time.

## Layers

- aspnetcore.ntier.API - Presentation Layer is type of *.Net Core Web API project*.
- aspnetcore.ntier.BLL - Business Logic Layer responsible for data exchange between DAL and Presentation Layer.
  - It has Services, AutoMapperProfiles and CustomExceptions in it
- aspnetcore.ntier.DAL - Data Access Layer responsible for interacting database. *Generic repositories* have been used.
  - Database context, repositories and database entity models are located in this class lib
- aspnetcore.ntier.DTO - Data transfer objects are added here
- aspnetcore.ntier.Test - Unit and Integration tests are created here

## Development process

You can follow the steps during development from the [commit list](https://github.com/aghayeffemin/aspnetcore.ntier/commits/master).

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## Support

If you found this project useful or interesting and would like to support my work you can support me. Thanks!

<a href="https://www.buymeacoffee.com/aghayeffemin" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/default-orange.png" alt="Buy Me A Coffee" width="200"></a>