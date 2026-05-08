using TL.DAL;
using TL.BLL;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using TL.DAL.Persistence;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;

Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

var connectionString = $"Host={Env.GetString("DB_HOST", "localhost")};" +
                       $"Port={Env.GetString("DB_PORT", "5432")};" +
                       $"Database={Env.GetString("POSTGRES_DB", "ThirdLabDb")};" +
                       $"Username={Env.GetString("POSTGRES_USER")};" +
                       $"Password={Env.GetString("POSTGRES_PASSWORD")};";

builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDataAccess(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        await context.Database.MigrateAsync();
        Console.WriteLine("Database migrated successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Migration failed: {ex.Message}");
    }
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();