using TL.BLL.Interfaces;
using TL.DAL.Interfaces;
using TL.BLL.DTOs;
using TL.DAL.Entities;
using AutoMapper;

namespace TL.BLL.Services;

public class RoomCategoryService : BaseService, IRoomCategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RoomCategoryService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> CreateAsync(CreateRoomCategoryRequest request)
    {
        await ValidateAsync(request);

        if (await _unitOfWork.Categories.ExistsByNameAsync(request.Name))
            throw new InvalidOperationException($"Category with name {request.Name} already exists.");

        var category = RoomCategory.Create(
            request.Name,
            request.PricePerNight
        );

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.CommitTransactionAsync();

            return category.Id;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task UpdateAsync(UpdateRoomCategoryRequest request)
    {
        await ValidateAsync(request);

        var category = await _unitOfWork.Categories.GetByIdAsync(request.Id)
                        ?? throw new KeyNotFoundException($"Category with ID {request.Id} is not found.");

        bool hasChanges = false;

        if (request.Name != null && request.Name != category.Name)
        {
            if (await _unitOfWork.Categories.ExistsByNameAsync(request.Name, category.Id))
                throw new InvalidOperationException($"Category with name {request.Name} already exists.");

            category.CorrectName(request.Name);
            hasChanges = true;
        }

        if (request.PricePerNight != null && request.PricePerNight.Value != category.PricePerNight)
        {
            category.CorrectPrice(request.PricePerNight.Value);
            hasChanges = true;
        }

        if (!hasChanges) return;

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            _unitOfWork.Categories.Update(category);
            await _unitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task DeleteAsync(Guid categoryId)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(categoryId)
                ?? throw new KeyNotFoundException($"Category with ID {categoryId} is not found.");

        if (await _unitOfWork.Rooms.HasAnyForCategoryAsync(categoryId))
            throw new InvalidOperationException("Cannot delete category with rooms.");

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            _unitOfWork.Categories.Delete(category);
            await _unitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<RoomCategoryResponse?> GetByIdAsync(Guid categoryId)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(categoryId)
                        ?? throw new KeyNotFoundException($"Category with ID {categoryId} is not found.");

        return _mapper.Map<RoomCategoryResponse>(category);
    }

    public async Task<List<RoomCategoryResponse>> GetAllAsync()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync();

        return _mapper.Map<List<RoomCategoryResponse>>(categories);
    }
}