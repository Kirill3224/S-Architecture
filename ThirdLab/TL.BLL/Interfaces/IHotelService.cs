using TL.BLL.DTOs;

namespace TL.BLL.Interfaces;

public interface IHotelService
{
    Task<IEnumerable<RoomResponse>> GetAvailableRoomsAsync(DateTime start, DateTime end);
    Task<BookingResponse> BookRoomAsync(BookRoomRequest request);
    Task CancelBookingAsync(Guid bookingId);
}