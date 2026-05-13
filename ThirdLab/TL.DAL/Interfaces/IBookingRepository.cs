using TL.DAL.Entities;

namespace TL.DAL.Interfaces;

public interface IBookingRepository : IBaseRepository<Booking>
{
    Task<bool> HasOverlappingBookingAsync(Guid roomId, DateTime startDate, DateTime endDate, Guid? id);
    Task<IEnumerable<Booking>> GetBookingsByRoomAsync(Guid roomId);
}