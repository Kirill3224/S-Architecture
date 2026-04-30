namespace TL.BLL.DTOs;

public record BookingResponse(
    Guid Id,
    string RoomNumber,
    string GuestName,
    DateTime StartDate,
    DateTime EndDate,
    decimal TotalPrice
);