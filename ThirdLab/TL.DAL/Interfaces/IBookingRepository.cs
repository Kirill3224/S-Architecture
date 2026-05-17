using TL.DAL.Entities;

namespace TL.DAL.Interfaces;

public interface IBookingRepository : IBaseRepository<Booking>
{
    Task<bool> HasOverlappingBookingAsync(Guid roomId, DateTime startDate, DateTime endDate, Guid? id, CancellationToken cancellationToken = default);
    Task<bool> HasAnyForRoomAsync(Guid roomId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Booking>> GetBookingsByRoomAsync(Guid roomId, CancellationToken cancellationToken = default);

}