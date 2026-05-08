using TL.BLL.Interfaces;
using TL.DAL.Interfaces;
using TL.BLL.DTOs;
using TL.DAL.Entities;
using AutoMapper;

namespace TL.BLL.Services;

public class BookingService : BaseService, IBookingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BookingService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> CreateAsync(CreateBookingRequest request)
    {
        await ValidateAsync(request);

        var room = await _unitOfWork.Rooms.GetByIdAsync(request.RoomId)
               ?? throw new KeyNotFoundException($"Room with ID {request.RoomId} is not found.");

        var booking = Booking.Create(
            room,
            request.GuestName,
            request.StartDate,
            request.EndDate
        );

        await _unitOfWork.Bookings.AddAsync(booking);
        await _unitOfWork.SaveChangesAsync();

        return booking.Id;
    }

    public async Task UpdateAsync(UpdateBookingRequest request)
    {
        await ValidateAsync(request);

        var booking = await _unitOfWork.Bookings.GetByIdAsync(request.Id)
                        ?? throw new KeyNotFoundException($"Booking with ID {request.Id} is not found.");

        bool hasChanges = false;

        if (request.GuestName != booking.GuestName)
        {
            booking.CorrectGuestName(request.GuestName);
            hasChanges = true;
        }

        if (request.StartDate != booking.StartDate)
        {
            booking.CorrectStartDate(request.StartDate);
            hasChanges = true;
        }

        if (request.EndDate != booking.EndDate)
        {
            booking.CorrectEndDate(request.EndDate);
            hasChanges = true;
        }

        if (!hasChanges) return;

        _unitOfWork.Bookings.Update(booking);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid bookingId)
    {
        var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId)
                ?? throw new KeyNotFoundException($"Booking with ID {bookingId} is not found.");

        _unitOfWork.Bookings.Delete(booking);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<BookingResponse?> GetByIdAsync(Guid bookingId)
    {
        var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId)
                ?? throw new KeyNotFoundException($"Booking with ID {bookingId} is not found.");

        return _mapper.Map<BookingResponse>(booking);
    }

    public async Task<List<BookingResponse>> GetBookingsByRoomAsync(Guid roomId)
    {
        var bookings = await _unitOfWork.Bookings.GetBookingsByRoomAsync(roomId);

        return _mapper.Map<List<BookingResponse>>(bookings);
    }

    public async Task<List<BookingResponse>> GetAllAsync()
    {
        var bookings = await _unitOfWork.Bookings.GetAllAsync();

        return _mapper.Map<List<BookingResponse>>(bookings);
    }
}