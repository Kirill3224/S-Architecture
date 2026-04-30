using TL.DAL.Interfaces;
using TL.DAL.Persistence;
using TL.DAL.Repositories;

namespace TL.DAL.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public IRoomCategoryRepository Categories { get; private set; }
    public IRoomRepository Rooms { get; private set; }
    public IBookingRepository Bookings { get; private set; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;

        Categories = new RoomCategoryRepository(_context);
        Rooms = new RoomRepository(_context);
        Bookings = new BookingRepository(_context);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}