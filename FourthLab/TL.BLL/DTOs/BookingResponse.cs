namespace TL.BLL.DTOs;

public record BookingResponse
{
    public Guid Id { get; init; }
    public string RoomNumber { get; init; } = null!;
    public string GuestName { get; init; } = null!;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public decimal TotalPrice { get; init; }
}