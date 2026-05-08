using TL.BLL.DTOs;

namespace TL.BLL.Interfaces;

public interface IBookingService
{
    Task<Guid> CreateAsync(CreateBookingRequest request);
    Task UpdateAsync(UpdateBookingRequest request);
    Task DeleteAsync(Guid bookingId);
    Task<BookingResponse?> GetByIdAsync(Guid bookingId);
    Task<List<BookingResponse>> GetBookingsByRoomAsync(Guid roomId);
    Task<List<BookingResponse>> GetAllAsync();
}