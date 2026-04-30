namespace TL.BLL.DTOs;

public record RoomResponse(
    Guid Id,
    string Number,
    string Status,
    string CategoryName,
    decimal PricePerNight
);