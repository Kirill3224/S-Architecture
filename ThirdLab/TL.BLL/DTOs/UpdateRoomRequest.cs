namespace TL.BLL.DTOs;

public record UpdateRoomRequest(
    Guid Id,
    string Number,
    string Status
);