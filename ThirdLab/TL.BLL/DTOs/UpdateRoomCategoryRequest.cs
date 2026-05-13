namespace TL.BLL.DTOs;

public record UpdateRoomCategoryRequest(
    Guid Id,
    string? Name,
    decimal? PricePerNight
);