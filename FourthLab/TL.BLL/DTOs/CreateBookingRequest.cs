using TL.DAL.Entities;

namespace TL.BLL.DTOs;

public record CreateBookingRequest(
    Guid RoomId,
    string GuestName,
    DateTime StartDate,
    DateTime EndDate
);