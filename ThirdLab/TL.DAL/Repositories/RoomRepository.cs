using Microsoft.EntityFrameworkCore;
using TL.DAL.Entities;
using TL.DAL.Interfaces;
using TL.DAL.Persistence;

namespace TL.DAL.Repositories;

public class RoomRepository : BaseRepository<Room>, IRoomRepository
{
    public RoomRepository(AppDbContext context) : base(context) { }

    public override async Task<IEnumerable<Room>> GetAllAsync()
    {
        return await _dbSet
            .Include(b => b.Category)
            .ToListAsync();
    }

    public override async Task<Room?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(r => r.Category)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
                .Include(r => r.Category)
                .Where(r => !r.Bookings.Any(b =>
                    b.StartDate < endDate && b.EndDate > startDate))
                .ToListAsync();
    }

    public async Task<bool> ExistsByNumberAsync(string number, Guid? id = null)
    {
        return await _dbSet.AnyAsync(r => r.Number == number && r.Id != id);
    }

    public async Task<bool> HasAnyForCategoryAsync(Guid categoryId)
    {
        return await _dbSet.AnyAsync(r => r.CategoryId == categoryId);
    }
}