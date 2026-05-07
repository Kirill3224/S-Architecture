using TL.BLL.Interfaces;
using TL.BLL.DTOs;
using TL.DAL.Interfaces;
using AutoMapper;
using TL.DAL.Entities;

namespace TL.BLL.Services;

public class HotelService : IHotelService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public HotelService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoomResponse>> GetAvailableRoomsAsync(DateTime start, DateTime end)
    {
        var rooms = await _unitOfWork.Rooms.GetAvailableRoomsAsync(start, end);

        return _mapper.Map<IEnumerable<RoomResponse>>(rooms);
    }

    public async Task<BookingResponse> BookRoomAsync(BookRoomRequest request)
    {
        bool isOccupied = await _unitOfWork.Bookings.HasOverlappingBookingAsync(
            request.RoomId, request.StartDate, request.EndDate);

        if (isOccupied)
            throw new InvalidOperationException("This room is already taken by someone else.");

        var room = await _unitOfWork.Rooms.GetWithCategoryAsync(request.RoomId);

        if (room is null)
            throw new KeyNotFoundException($"Room with ID {request.RoomId} is not found.");

        var booking = Booking.Create(room, request.GuestName, request.StartDate, request.EndDate);

        room.MarkAsBooked();

        await _unitOfWork.Bookings.AddAsync(booking);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<BookingResponse>(booking);
    }

    public async Task CancelBookingAsync(Guid bookingId)
    {
        var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId);

        if (booking is null)
            throw new KeyNotFoundException($"Booking with ID {bookingId} is not found.");

        var room = await _unitOfWork.Rooms.GetWithCategoryAsync(booking.RoomId);

        if (room is null)
            throw new KeyNotFoundException($"Room with ID {booking.RoomId} is not found.");

        room.MarkAsFree();

        _unitOfWork.Bookings.Delete(booking);
        await _unitOfWork.SaveChangesAsync();
    }
}