using DotNetEnv;
using TL.DAL.Persistence;
using Microsoft.EntityFrameworkCore;

Env.TraversePath().Load();

var dbHost = Env.GetString("DB_HOST", "localhost");
var dbPort = Env.GetString("DB_PORT", "5432");
var dbUser = Env.GetString("POSTGRES_USER");
var dbPass = Env.GetString("POSTGRES_PASSWORD");
var dbName = Env.GetString("POSTGRES_DB", "ThirdLabDb");

var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPass};";

var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionsBuilder.UseNpgsql(connectionString);

using var context = new AppDbContext(optionsBuilder.Options);

try
{
    context.Database.Migrate();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}