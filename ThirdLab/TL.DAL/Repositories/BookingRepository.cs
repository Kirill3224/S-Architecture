using Microsoft.EntityFrameworkCore;
using TL.DAL.Entities;
using TL.DAL.Interfaces;
using TL.DAL.Persistence;

namespace TL.DAL.Repositories;

public class BookingRepository : BaseRepository<Booking>, IBookingRepository
{
    public BookingRepository(AppDbContext context) : base(context) { }

    public override async Task<IEnumerable<Booking>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(b => b.Room)
                .ThenInclude(r => r.Category)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasOverlappingBookingAsync(Guid roomId, DateTime startDate, DateTime endDate, Guid? id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(b =>
                b.RoomId == roomId &&
                b.StartDate < endDate &&
                b.EndDate > startDate &&
                b.Id != id,
                cancellationToken);
    }

    public async Task<bool> HasAnyForRoomAsync(Guid roomId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(b => b.RoomId == roomId, cancellationToken);
    }

    public async Task<IEnumerable<Booking>> GetBookingsByRoomAsync(Guid roomId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
                .Include(b => b.Room)
                    .ThenInclude(r => r.Category)
                .Where(b => b.RoomId == roomId)
                .ToListAsync(cancellationToken);
    }
}