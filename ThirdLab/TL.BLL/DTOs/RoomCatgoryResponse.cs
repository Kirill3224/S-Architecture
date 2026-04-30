namespace TL.BLL.DTOs;

public record RoomCategoryResponse(
    Guid Id,
    string Name,
    decimal PricePerNight
);