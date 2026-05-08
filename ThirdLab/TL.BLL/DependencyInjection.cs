using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using TL.BLL.Interfaces;
using TL.BLL.Services;
using TL.BLL.Validators.Room;
using System.Reflection;


namespace TL.BLL;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IHotelService, HotelService>();
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IRoomCategoryService, RoomCategoryService>();
        services.AddScoped<IBookingService, BookingService>();

        services.AddValidatorsFromAssemblyContaining<CreateRoomValidator>();

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}