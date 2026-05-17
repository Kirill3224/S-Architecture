using TL.DAL.Entities;
using TL.DAL.Interfaces;
using TL.DAL.Persistence;
using Microsoft.EntityFrameworkCore;

namespace TL.DAL.Repositories;

public class RoomCategoryRepository : BaseRepository<RoomCategory>, IRoomCategoryRepository
{
    public RoomCategoryRepository(AppDbContext context) : base(context) { }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken, Guid? id = null)
    {
        return await _dbSet.AnyAsync(rc => rc.Name == name && rc.Id != id, cancellationToken);
    }
}