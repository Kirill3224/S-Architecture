namespace TL.DAL.Entities;

public class Booking : BaseEntity
{
    public Guid RoomId { get; private set; }
    public string GuestName { get; private set; } = null!;
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public Room Room { get; private set; } = null!;

    protected Booking() { }

    private Booking(Guid roomId, string guestName, DateTime startDate, DateTime endDate)
    {
        RoomId = roomId;
        GuestName = guestName;
        StartDate = startDate;
        EndDate = endDate;
    }

    public static Booking Create(Guid roomId, string guestName, DateTime startDate, DateTime endDate)
    {
        if (string.IsNullOrWhiteSpace(guestName))
            throw new ArgumentException("Guest name is required", nameof(guestName));

        if (endDate <= startDate)
            throw new ArgumentException("End date must be after start date.", nameof(endDate));

        if (startDate < DateTime.UtcNow.Date)
            throw new ArgumentException("Start date cannot be in the past.", nameof(startDate));

        return new Booking(roomId, guestName, startDate, endDate);
    }
}