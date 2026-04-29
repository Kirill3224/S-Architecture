namespace TL.DAL.Entities;

public class RoomCategory : BaseEntity
{
    public string Name { get; private set; } = null!;
    public decimal PricePerNight { get; private set; }
    public ICollection<Room> Rooms { get; private set; }

    protected RoomCategory()
    {
        Rooms = new List<Room>();
    }

    private RoomCategory(string name, decimal pricePerNight) : this()
    {
        Name = name;
        PricePerNight = pricePerNight;
    }

    public RoomCategory Create(string name, decimal pricePerNight)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));
        if (pricePerNight < 0)
            throw new ArgumentException("Price cannot be negative.", nameof(pricePerNight));

        return new RoomCategory(name, pricePerNight);
    }
}