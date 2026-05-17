namespace TL.BLL.DTOs;

public record BookRoomRequest(
    Guid RoomId,
    string GuestName,
    DateTime StartDate,
    DateTime EndDate
);