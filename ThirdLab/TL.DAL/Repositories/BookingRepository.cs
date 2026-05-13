using Microsoft.EntityFrameworkCore;
using TL.DAL.Entities;
using TL.DAL.Interfaces;
using TL.DAL.Persistence;

namespace TL.DAL.Repositories;

public class BookingRepository : BaseRepository<Booking>, IBookingRepository
{
    public BookingRepository(AppDbContext context) : base(context) { }

    public override async Task<IEnumerable<Booking>> GetAllAsync()
    {
        return await _dbSet
            .Include(b => b.Room)
                .ThenInclude(r => r.Category)
            .ToListAsync();
    }

    public async Task<bool> HasOverlappingBookingAsync(Guid roomId, DateTime startDate, DateTime endDate)
    {
        return await _dbSet.AnyAsync(b =>
                b.RoomId == roomId &&
                b.StartDate < endDate &&
                b.EndDate > startDate);
    }

    public async Task<IEnumerable<Booking>> GetBookingsByRoomAsync(Guid roomId)
    {
        return await _dbSet
                .Include(b => b.Room)
                    .ThenInclude(r => r.Category)
                .Where(b => b.RoomId == roomId)
                .ToListAsync();
    }
}