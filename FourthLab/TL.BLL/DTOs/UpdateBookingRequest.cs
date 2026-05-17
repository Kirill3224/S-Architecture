using TL.DAL.Entities;

namespace TL.BLL.DTOs;

public record UpdateBookingRequest(
    Guid Id,
    string? GuestName,
    DateTime? StartDate,
    DateTime? EndDate
);