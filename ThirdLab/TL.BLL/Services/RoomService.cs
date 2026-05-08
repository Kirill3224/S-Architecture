using TL.BLL.Interfaces;
using TL.DAL.Interfaces;
using TL.BLL.DTOs;
using TL.DAL.Enums;
using TL.DAL.Entities;
using AutoMapper;
using FluentValidation;

namespace TL.BLL.Services;

public class RoomService : IRoomService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateRoomRequest> _createValidator;
    private readonly IValidator<UpdateRoomRequest> _updateValidator;

    public RoomService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateRoomRequest> createValidator,
        IValidator<UpdateRoomRequest> updateValidator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<Guid> CreateAsync(CreateRoomRequest request)
    {
        await _createValidator.ValidateAndThrowAsync(request);

        var status = Enum.Parse<RoomStatus>(request.Status, ignoreCase: true);

        var room = Room.Create(
            request.Number,
            request.CategoryId,
            status
        );

        await _unitOfWork.Rooms.AddAsync(room);
        await _unitOfWork.SaveChangesAsync();

        return room.Id;
    }

    public async Task UpdateAsync(UpdateRoomRequest request)
    {
        await _updateValidator.ValidateAndThrowAsync(request);

        var room = await _unitOfWork.Rooms.GetByIdAsync(request.Id)
                    ?? throw new KeyNotFoundException($"Room with ID {request.Id} is not found.");

        var status = Enum.Parse<RoomStatus>(request.Status, ignoreCase: true);

        bool hasChanges = false;

        if (request.Number != room.Number)
        {
            room.CorrectNumber(request.Number);
            hasChanges = true;
        }

        if (status != room.Status)
        {
            room.CorrectStatus(status);
            hasChanges = true;
        }

        if (!hasChanges) return;

        _unitOfWork.Rooms.Update(room);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid roomId)
    {
        var room = await _unitOfWork.Rooms.GetByIdAsync(roomId)
                    ?? throw new KeyNotFoundException($"Room with ID {roomId} is not found.");



        _unitOfWork.Rooms.Delete(room);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<RoomResponse?> GetByIdAsync(Guid roomId)
    {
        var room = await _unitOfWork.Rooms.GetWithCategoryAsync(roomId)
                    ?? throw new KeyNotFoundException($"Room with ID {roomId} is not found.");

        return _mapper.Map<RoomResponse>(room);
    }

    public async Task<List<RoomResponse>> GetAllAsync()
    {
        var rooms = await _unitOfWork.Rooms.GetAllAsync();

        return _mapper.Map<List<RoomResponse>>(rooms);
    }
}
