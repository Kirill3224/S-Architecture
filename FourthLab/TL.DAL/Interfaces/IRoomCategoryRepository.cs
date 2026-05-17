using TL.DAL.Entities;

namespace TL.DAL.Interfaces;

public interface IRoomCategoryRepository : IBaseRepository<RoomCategory>
{
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default, Guid? id = null);
}