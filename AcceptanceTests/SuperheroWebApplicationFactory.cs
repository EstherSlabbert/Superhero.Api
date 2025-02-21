﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Superhero;
using Superhero.Data;
using Superhero.Repositories;
using Superhero.Services;

namespace AcceptanceTests
{
    public class SuperheroWebApplicationFactory : WebApplicationFactory<IAssemblyMarker>
    {
        private void Configure(IConfigurationBuilder builder)
        {
            builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        }

        //overrides host configuration used for tests, using configure defined above
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureHostConfiguration(Configure);
            return base.CreateHost(builder);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                // Remove existing database context registration
                services.RemoveAll<DataContext>();
                services.RemoveAll<DbContextOptions<DataContext>>();

                // Get configuration
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var testDbConnectionString = configuration.GetConnectionString("TestConnection");

                // Register DbContext with the test database connection
                services.AddDbContext<DataContext>(options =>
                {
                    options.UseSqlServer(testDbConnectionString);
                });

                // Ensure database is created and migrated
                var dbContextFactory = services.BuildServiceProvider();
                using var dbScope = dbContextFactory.CreateScope();
                var dbContext = dbScope.ServiceProvider.GetRequiredService<DataContext>();
                dbContext.Database.EnsureDeleted();
                dbContext.Database.Migrate();
                dbContext.Database.EnsureCreated();

                // Register services as needed
                services.AddScoped<ISuperHeroRepository, SuperHeroRepository>();
                services.AddScoped<ISuperHeroService, SuperHeroService>(); /*Services created once per request i.e. new instance for each http request*/
                //services.AddSingleton<SuperHeroService>(); /*Created once and shared throughout app's lifetime i.e. shared across requests*/
            });
        }
    }
}
