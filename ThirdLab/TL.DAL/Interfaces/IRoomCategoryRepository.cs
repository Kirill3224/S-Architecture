using TL.DAL.Entities;

namespace TL.DAL.Interfaces;

public interface IRoomCategoryRepository : IBaseRepository<RoomCategory>
{
    Task<bool> ExistsByNameAsync(string name, Guid? id = null);
}