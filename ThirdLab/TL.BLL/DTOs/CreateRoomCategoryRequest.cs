namespace TL.BLL.DTOs;

public record CreateRoomCategoryRequest(
    string Name,
    decimal PricePerNight
);