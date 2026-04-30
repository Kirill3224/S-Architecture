using TL.DAL.Entities;

namespace TL.DAL.Interfaces;

public interface IUnitOfWork
{
    public IRoomCategoryRepository Categories { get; }
    public IRoomRepository Rooms { get; }
    public IBookingRepository Bookings { get; }
    Task<int> SaveChangesAsync();
}