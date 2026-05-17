using TL.BLL.Interfaces;
using TL.DAL.Interfaces;
using TL.BLL.DTOs;
using TL.DAL.Enums;
using TL.DAL.Entities;
using AutoMapper;

namespace TL.BLL.Services;

public class RoomService : BaseService, IRoomService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RoomService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> CreateAsync(CreateRoomRequest request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request);

        if (await _unitOfWork.Rooms.ExistsByNumberAsync(request.Number, cancellationToken))
            throw new InvalidOperationException($"Room with number {request.Number} already exists.");

        var status = Enum.Parse<RoomStatus>(request.Status, ignoreCase: true);

        var room = Room.Create(
            request.Number,
            request.CategoryId,
            status
        );

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await _unitOfWork.Rooms.AddAsync(room, cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return room.Id;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(CancellationToken.None);
            throw;
        }
    }

    public async Task UpdateAsync(UpdateRoomRequest request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request);

        var room = await _unitOfWork.Rooms.GetByIdAsync(request.Id, cancellationToken)
                    ?? throw new KeyNotFoundException($"Room with ID {request.Id} is not found.");

        bool hasChanges = false;

        if (request.Number != null && request.Number != room.Number)
        {
            if (await _unitOfWork.Rooms.ExistsByNumberAsync(request.Number, cancellationToken, room.Id))
                throw new InvalidOperationException($"Room with number {request.Number} already exists.");

            room.CorrectNumber(request.Number);
            hasChanges = true;
        }

        if (request.Status != null)
        {
            var status = Enum.Parse<RoomStatus>(request.Status, ignoreCase: true);
            if (status != room.Status)
            {
                room.CorrectStatus(status);
                hasChanges = true;
            }
        }

        if (!hasChanges) return;

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            _unitOfWork.Rooms.Update(room);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(CancellationToken.None);
            throw;
        }
    }

    public async Task DeleteAsync(Guid roomId, CancellationToken cancellationToken)
    {
        var room = await _unitOfWork.Rooms.GetByIdAsync(roomId, cancellationToken)
                    ?? throw new KeyNotFoundException($"Room with ID {roomId} is not found.");

        if (await _unitOfWork.Bookings.HasAnyForRoomAsync(room.Id, cancellationToken))
            throw new InvalidOperationException("Cannot delete room with active booking.");

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            _unitOfWork.Rooms.Delete(room);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(CancellationToken.None);
            throw;
        }
    }

    public async Task<RoomResponse?> GetByIdAsync(Guid roomId, CancellationToken cancellationToken)
    {
        var room = await _unitOfWork.Rooms.GetByIdAsync(roomId, cancellationToken)
                    ?? throw new KeyNotFoundException($"Room with ID {roomId} is not found.");

        return _mapper.Map<RoomResponse>(room);
    }

    public async Task<List<RoomResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        var rooms = await _unitOfWork.Rooms.GetAllAsync(cancellationToken);

        return _mapper.Map<List<RoomResponse>>(rooms);
    }
}
