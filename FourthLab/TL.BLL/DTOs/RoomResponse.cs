namespace TL.BLL.DTOs;

public record RoomResponse
{
    public Guid Id { get; init; }
    public string Number { get; init; } = null!;
    public string Status { get; init; } = null!;
    public string CategoryName { get; init; } = null!;
    public decimal PricePerNight { get; init; }
};