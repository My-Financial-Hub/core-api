# My Financial Hub
This is a completely experimental project, a lot of stuff here was not made with the purpose of being used in production environment.
Until v1 release, everything of this project could be changed.

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=My-Financial-Hub_core-api&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=My-Financial-Hub_core-api)   

## What's planned for the future
- **[Monthly Milestones](https://github.com/Chingling152/my-financial-hub/issues/9)**
- **[Earns/Expenses Portion](https://github.com/Chingling152/my-financial-hub/issues/10)**
- **Transaction relatories**
- **Dashboards**
- **Earns/Expenses average**
- **Earns/Expenses predictions**

## Requeriments
  * Docker (optional)
  * .NET 6.0
  * SQL Server 

## How to Start
* Run the Dockerfile in root project to create the database (optional)
* Configure the **ConnectionStrings** in the file **appsettings.Development.json** with your SQL Server database
* Initial configuration (pick one)
  * #### Visual Studio
    Open : Project solution and build the project
    Open : Tools -> NuGet Package Manager -> Package Manager Console
    Type : Update-Database
  * ### Dotnet Cli
    Open : WebApi Project directory
    Type : dotnet build
    Type : dotnet tool install --global dotnet-ef
    Type : dotnet ef database update
* After that, you'll be able to run the project
