using AutoMapper;
using TL.BLL.DTOs;
using TL.DAL.Entities;

namespace TL.BLL;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RoomCategory, RoomCategoryResponse>();

        CreateMap<Room, RoomResponse>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.PricePerNight, opt => opt.MapFrom(src => src.Category.PricePerNight));

        CreateMap<Booking, BookingResponse>()
            .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Room.Number))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src =>
                (src.EndDate - src.StartDate).Days * src.Room.Category.PricePerNight));
    }
}