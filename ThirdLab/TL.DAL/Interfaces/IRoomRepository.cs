using TL.DAL.Entities;

namespace TL.DAL.Interfaces;

public interface IRoomRepository : IBaseRepository<Room>
{
    Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNumberAsync(string number, Guid? id = null, CancellationToken cancellationToken = default);
    Task<bool> HasAnyForCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default);
}
