# Superhero API Web App

ASP.NET Web API SuperHero App using SqlServer database.

## Initial set up and discovery

1. Create a new project with VisualStudio selecting the following settings:

   Template: ASP.NET Core Web API

   Name: Whatever you want to name your project

   Framework: .NET 8.0 (Long Term Support)

   Authentication: None

   - [x] Configure for HTTPS
   - [ ] Enable container support
   - [x] Enable OpenApi support
   - [ ] Do not use top level statements
   - [x] Use controllers
   - [ ] Enlist in .NET Aspire orchestration

2. Use NuGet Package Manager to install:

   - Microsoft.EntityFrameworkCore.SqlServer (Version 8.0.0)
   - Microsoft.EntityFrameworkCore.Tools (Version 8.0.0)

3. Set up a new entity (in this project, a class called `SuperHero`) with the necessary properties.
4. Add a new controller for getting all the superheroes with dummy data to return from the controller itself as a placeholder for now.
5. Set up your `DataContext` file, which creates a class that inherits from `DbContext` and add your context options to the constructor and a new table in the form of `DbSet<SuperHero>`.
6. Set your connection strings in the `appsettings.json` file.
7. Update `Program.cs` file with the database context like follows:

   ```csharp
        builder.Services.AddDbContext<DataContext>(options =>
   {
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
   });
   ```

8. Open the Package Manager Console and enter the following commands:

   `Add-Migration Initial` Initial is the name of the migration that will be stored in the Migrations folder

   then `Update-Database`

   Note: These commands ensure the database has the needful table(s) and when new tables need to be added similar commands need to be run from the Package Manager Console `Add-Migration NameOfMigration` then `Update-Database` OR via the command line like `dotnet ef migrations add AddUnhandledExceptionTable --project Superhero --startup-project Superhero` then `dotnet ef database update`

9. Now set up the controller to use the database with the DataContext as an entry point. Get the controller working as desired then add other controllers for CRUD operations.
10. Clean up unneeded files.
11. Separate out files and methods into projects as dependency injections and services to be used etc.
