using TL.DAL.Entities;

namespace TL.DAL.Interfaces;

public interface IRoomRepository : IBaseRepository<Room>
{
    Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime startDate, DateTime endDate);
}
