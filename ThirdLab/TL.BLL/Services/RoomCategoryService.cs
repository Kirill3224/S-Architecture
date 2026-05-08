using TL.BLL.Interfaces;
using TL.DAL.Interfaces;
using TL.BLL.DTOs;
using TL.DAL.Entities;
using AutoMapper;

namespace TL.BLL.Services;

public class RoomCategoryService : IRoomCategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RoomCategoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> CreateAsync(CreateRoomCategoryRequest request)
    {
        var category = RoomCategory.Create(
            request.Name,
            request.PricePerNight
        );

        await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return category.Id;
    }

    public async Task UpdateAsync(UpdateRoomCategoryRequest request)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(request.Id)
                        ?? throw new KeyNotFoundException($"Category with ID {request.Id} is not found.");

        bool hasChanges = false;

        if (request.Name != category.Name)
        {
            category.CorrectName(request.Name);
            hasChanges = true;
        }

        if (request.PricePerNight != category.PricePerNight)
        {
            category.CorrectPrice(request.PricePerNight);
            hasChanges = true;
        }

        if (!hasChanges) return;

        _unitOfWork.Categories.Update(category);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid roomCategoryId)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(roomCategoryId)
                ?? throw new KeyNotFoundException($"Category with ID {roomCategoryId} is not found.");

        _unitOfWork.Categories.Delete(category);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<RoomCategoryResponse?> GetRoomByIdAsync(Guid roomCategoryId)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(roomCategoryId)
                        ?? throw new KeyNotFoundException($"Category with ID {roomCategoryId} is not found.");

        return _mapper.Map<RoomCategoryResponse>(category);
    }

    public async Task<List<RoomCategoryResponse>> GetAllAsync()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync();

        return _mapper.Map<List<RoomCategoryResponse>>(categories);
    }
}