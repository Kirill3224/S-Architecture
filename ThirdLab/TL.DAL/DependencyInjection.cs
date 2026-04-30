using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using TL.DAL.Persistence;
using TL.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using TL.DAL.Interfaces;

namespace TL.DAL;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}