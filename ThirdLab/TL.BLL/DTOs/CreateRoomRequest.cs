namespace TL.BLL.DTOs;

public record CreateRoomRequest(
    string Number,
    Guid CategoryId,
    string Status
);