namespace TL.BLL.DTOs;

public record SearchAvailableRoomsRequest(
    DateTime StartDate,
    DateTime EndDate
);