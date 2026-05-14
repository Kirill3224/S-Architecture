using TL.DAL.Entities;

namespace TL.DAL.Interfaces;

public interface IRoomRepository : IBaseRepository<Room>
{
    Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime startDate, DateTime endDate);
    Task<bool> ExistsByNumberAsync(string number, Guid? id = null);
    Task<bool> HasAnyForCategoryAsync(Guid categoryId);
}
