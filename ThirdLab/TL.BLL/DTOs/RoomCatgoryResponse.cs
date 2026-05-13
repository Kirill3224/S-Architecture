namespace TL.BLL.DTOs;

public record RoomCategoryResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public decimal PricePerNight { get; init; }
}