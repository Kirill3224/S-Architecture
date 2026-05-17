using Microsoft.EntityFrameworkCore;
using TL.DAL.Entities;
using TL.DAL.Interfaces;
using TL.DAL.Persistence;

namespace TL.DAL.Repositories;

public class RoomRepository : BaseRepository<Room>, IRoomRepository
{
    public RoomRepository(AppDbContext context) : base(context) { }

    public override async Task<IEnumerable<Room>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbSet
            .Include(b => b.Category)
            .ToListAsync(cancellationToken);
    }

    public override async Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbSet
            .Include(r => r.Category)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
    {
        return await _dbSet
                .Include(r => r.Category)
                .Where(r => !r.Bookings.Any(b =>
                    b.StartDate < endDate && b.EndDate > startDate))
                .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByNumberAsync(string number, CancellationToken cancellationToken, Guid? id = null)
    {
        return await _dbSet.AnyAsync(r => r.Number == number && r.Id != id, cancellationToken);
    }

    public async Task<bool> HasAnyForCategoryAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        return await _dbSet.AnyAsync(r => r.CategoryId == categoryId, cancellationToken);
    }
}