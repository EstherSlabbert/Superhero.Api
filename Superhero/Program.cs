using Microsoft.EntityFrameworkCore;
using Superhero.Data;
using Superhero.ErrorHandling;
using Superhero.Repositories;
using Superhero.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Register repositories and service(s)
builder.Services.AddScoped<ISuperHeroRepository, SuperHeroRepository>();
builder.Services.AddScoped<IUnhandledExceptionRepository, UnhandledExceptionRepository>();
builder.Services.AddScoped<ISuperHeroService, SuperHeroService>();

var app = builder.Build();

// Configure exception handling
app.MapExceptionsToProblemDetails();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
