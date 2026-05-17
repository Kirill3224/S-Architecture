using TL.BLL.DTOs;

namespace TL.BLL.Interfaces;

public interface IBookingService
{
    Task<BookingResponse> CreateAsync(CreateBookingRequest request, CancellationToken cancellationToken = default);
    Task UpdateAsync(UpdateBookingRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid bookingId, CancellationToken cancellationToken = default);
    Task<BookingResponse?> GetByIdAsync(Guid bookingId, CancellationToken cancellationToken = default);
    Task<List<BookingResponse>> GetBookingsByRoomAsync(Guid roomId, CancellationToken cancellationToken = default);
    Task<IEnumerable<RoomResponse>> GetAvailableRoomsAsync(SearchAvailableRoomsRequest request, CancellationToken cancellationToken = default);
    Task<List<BookingResponse>> GetAllAsync(CancellationToken cancellationToken = default);
}