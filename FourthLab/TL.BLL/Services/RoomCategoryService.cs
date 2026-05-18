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

    public async Task<Guid> CreateAsync(CreateRoomCategoryRequest request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request);

        if (await _unitOfWork.Categories.ExistsByNameAsync(request.Name, cancellationToken))
            throw new InvalidOperationException($"Category with name {request.Name} already exists.");

        var category = RoomCategory.Create(
            request.Name,
            request.PricePerNight
        );

        await _unitOfWork.Categories.AddAsync(category, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return category.Id;
    }

    public async Task UpdateAsync(UpdateRoomCategoryRequest request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request);

        var category = await _unitOfWork.Categories.GetByIdAsync(request.Id, cancellationToken)
                        ?? throw new KeyNotFoundException($"Category with ID {request.Id} is not found.");

        bool hasChanges = false;

        if (request.Name != null && request.Name != category.Name)
        {
            if (await _unitOfWork.Categories.ExistsByNameAsync(request.Name, cancellationToken, category.Id))
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

        _unitOfWork.Categories.Update(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(categoryId, cancellationToken)
                ?? throw new KeyNotFoundException($"Category with ID {categoryId} is not found.");

        if (await _unitOfWork.Rooms.HasAnyForCategoryAsync(categoryId, cancellationToken))
            throw new InvalidOperationException("Cannot delete category with rooms.");

        _unitOfWork.Categories.Delete(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<RoomCategoryResponse?> GetByIdAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(categoryId, cancellationToken)
                        ?? throw new KeyNotFoundException($"Category with ID {categoryId} is not found.");

        return _mapper.Map<RoomCategoryResponse>(category);
    }

    public async Task<List<RoomCategoryResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        var categories = await _unitOfWork.Categories.GetAllAsync(cancellationToken);

        return _mapper.Map<List<RoomCategoryResponse>>(categories);
    }
}