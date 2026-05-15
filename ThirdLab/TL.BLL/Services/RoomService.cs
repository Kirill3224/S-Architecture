using TL.BLL.Interfaces;
using TL.DAL.Interfaces;
using TL.BLL.DTOs;
using TL.DAL.Enums;
using TL.DAL.Entities;
using AutoMapper;
using FluentValidation;

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

    public async Task<Guid> CreateAsync(CreateRoomRequest request)
    {
        await ValidateAsync(request);

        if (await _unitOfWork.Rooms.ExistsByNumberAsync(request.Number))
            throw new InvalidOperationException($"Room with number {request.Number} already exists.");

        var status = Enum.Parse<RoomStatus>(request.Status, ignoreCase: true);

        var room = Room.Create(
            request.Number,
            request.CategoryId,
            status
        );

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _unitOfWork.Rooms.AddAsync(room);
            await _unitOfWork.CommitTransactionAsync();

            return room.Id;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task UpdateAsync(UpdateRoomRequest request)
    {
        await ValidateAsync(request);

        var room = await _unitOfWork.Rooms.GetByIdAsync(request.Id)
                    ?? throw new KeyNotFoundException($"Room with ID {request.Id} is not found.");

        bool hasChanges = false;

        if (request.Number != null && request.Number != room.Number)
        {
            if (await _unitOfWork.Rooms.ExistsByNumberAsync(request.Number, room.Id))
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

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            _unitOfWork.Rooms.Update(room);
            await _unitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task DeleteAsync(Guid roomId)
    {
        var room = await _unitOfWork.Rooms.GetByIdAsync(roomId)
                    ?? throw new KeyNotFoundException($"Room with ID {roomId} is not found.");

        if (await _unitOfWork.Bookings.HasAnyForRoomAsync(room.Id))
            throw new InvalidOperationException("Cannot delete room with active booking.");

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            _unitOfWork.Rooms.Delete(room);
            await _unitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<RoomResponse?> GetByIdAsync(Guid roomId)
    {
        var room = await _unitOfWork.Rooms.GetByIdAsync(roomId)
                    ?? throw new KeyNotFoundException($"Room with ID {roomId} is not found.");

        return _mapper.Map<RoomResponse>(room);
    }

    public async Task<List<RoomResponse>> GetAllAsync()
    {
        var rooms = await _unitOfWork.Rooms.GetAllAsync();

        return _mapper.Map<List<RoomResponse>>(rooms);
    }
}
