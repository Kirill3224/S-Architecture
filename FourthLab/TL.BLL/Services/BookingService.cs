using TL.BLL.Interfaces;
using TL.DAL.Interfaces;
using TL.BLL.DTOs;
using TL.DAL.Entities;
using TL.DAL.Enums;
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

    public async Task<BookingResponse> CreateAsync(CreateBookingRequest request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request);

        var startDate = DateTime.SpecifyKind(request.StartDate, DateTimeKind.Utc);
        var endDate = DateTime.SpecifyKind(request.EndDate, DateTimeKind.Utc);

        bool isOccupied = await _unitOfWork.Bookings.HasOverlappingBookingAsync(
                                request.RoomId, startDate, endDate, cancellationToken);

        if (isOccupied)
            throw new InvalidOperationException("This room is already taken by someone else.");

        var room = await _unitOfWork.Rooms.GetByIdAsync(request.RoomId, cancellationToken)
               ?? throw new KeyNotFoundException($"Room with ID {request.RoomId} is not found.");

        if (room.Status == RoomStatus.Occupied && startDate.Date <= DateTime.UtcNow)
        {
            throw new InvalidOperationException("Cannot book this room: it is currently occupied by a guest.");
        }

        var booking = Booking.Create(
            room,
            request.GuestName,
            startDate,
            endDate
        );

        room.MarkAsBooked();

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await _unitOfWork.Bookings.AddAsync(booking, cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(CancellationToken.None);
            throw;
        }

        return _mapper.Map<BookingResponse>(booking);
    }

    public async Task UpdateAsync(UpdateBookingRequest request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request);

        var booking = await _unitOfWork.Bookings.GetByIdAsync(request.Id, cancellationToken)
                        ?? throw new KeyNotFoundException($"Booking with ID {request.Id} is not found.");

        DateTime actualStart = request.StartDate.HasValue
            ? DateTime.SpecifyKind(request.StartDate.Value, DateTimeKind.Utc)
            : booking.StartDate;

        DateTime actualEnd = request.EndDate.HasValue
            ? DateTime.SpecifyKind(request.EndDate.Value, DateTimeKind.Utc)
            : booking.EndDate;

        if (actualEnd <= actualStart)
        {
            throw new InvalidOperationException("End date cannot be earlier than start date.");
        }

        if (request.StartDate.HasValue || request.EndDate.HasValue)
        {
            bool isOccupied = await _unitOfWork.Bookings.HasOverlappingBookingAsync(
                booking.RoomId, actualStart, actualEnd, cancellationToken, booking.Id);

            if (isOccupied) throw new InvalidOperationException("Room is occupied for these new dates.");
        }

        if (request.GuestName != null) booking.CorrectGuestName(request.GuestName);

        if (request.StartDate.HasValue) booking.CorrectStartDate(actualStart);
        if (request.EndDate.HasValue) booking.CorrectEndDate(actualEnd);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            _unitOfWork.Bookings.Update(booking);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(CancellationToken.None);
            throw;
        }
    }

    public async Task DeleteAsync(Guid bookingId, CancellationToken cancellationToken)
    {
        var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId, cancellationToken)
                ?? throw new KeyNotFoundException($"Booking with ID {bookingId} is not found.");

        var room = await _unitOfWork.Rooms.GetByIdAsync(booking.RoomId, cancellationToken)
                ?? throw new KeyNotFoundException($"Room with ID {booking.RoomId} is not found.");

        room?.MarkAsFree();

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            _unitOfWork.Bookings.Delete(booking);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(CancellationToken.None);
            throw;
        }
    }

    public async Task<BookingResponse?> GetByIdAsync(Guid bookingId, CancellationToken cancellationToken)
    {
        var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId, cancellationToken)
                ?? throw new KeyNotFoundException($"Booking with ID {bookingId} is not found.");

        return _mapper.Map<BookingResponse>(booking);
    }

    public async Task<List<BookingResponse>> GetBookingsByRoomAsync(Guid roomId, CancellationToken cancellationToken)
    {
        var bookings = await _unitOfWork.Bookings.GetBookingsByRoomAsync(roomId, cancellationToken);

        return _mapper.Map<List<BookingResponse>>(bookings);
    }

    public async Task<IEnumerable<RoomResponse>> GetAvailableRoomsAsync(SearchAvailableRoomsRequest request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request);

        var startDate = DateTime.SpecifyKind(request.StartDate, DateTimeKind.Utc);
        var endDate = DateTime.SpecifyKind(request.EndDate, DateTimeKind.Utc);

        var rooms = await _unitOfWork.Rooms.GetAvailableRoomsAsync(startDate, endDate, cancellationToken);

        return _mapper.Map<IEnumerable<RoomResponse>>(rooms);
    }

    public async Task<List<BookingResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        var bookings = await _unitOfWork.Bookings.GetAllAsync(cancellationToken);

        return _mapper.Map<List<BookingResponse>>(bookings);
    }
}