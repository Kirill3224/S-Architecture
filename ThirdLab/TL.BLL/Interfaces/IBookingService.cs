using TL.BLL.DTOs;

namespace TL.BLL.Interfaces;

public interface IBookingService
{
    Task<BookingResponse> CreateAsync(CreateBookingRequest request);
    Task UpdateAsync(UpdateBookingRequest request);
    Task DeleteAsync(Guid bookingId);
    Task<BookingResponse?> GetByIdAsync(Guid bookingId);
    Task<List<BookingResponse>> GetBookingsByRoomAsync(Guid roomId);
    Task<IEnumerable<RoomResponse>> GetAvailableRoomsAsync(SearchAvailableRoomsRequest request);
    Task<List<BookingResponse>> GetAllAsync();
}