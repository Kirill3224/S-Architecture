using TL.DAL.Enums;

namespace TL.DAL.Entities;

public class Room : BaseEntity
{
    public string Number { get; private set; } = null!;
    public RoomStatus Status { get; private set; }
    public Guid CategoryId { get; private set; }

    protected Room() { }

    private Room(string number, Guid categoryId, RoomStatus status = RoomStatus.Free)
    {
        Number = number;
        CategoryId = categoryId;
        Status = status;
    }

    public static Room Create(string number, Guid categoryId, RoomStatus status = RoomStatus.Free)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException("Number is required", nameof(number));

        return new Room(number, categoryId, status);
    }

}