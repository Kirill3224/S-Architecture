using TL.BLL.DTOs;

namespace TL.BLL.Interfaces;

public interface IRoomCategoryService
{
    Task<Guid> CreateAsync(CreateRoomCategoryRequest request, CancellationToken cancellationToken = default);
    Task UpdateAsync(UpdateRoomCategoryRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<RoomCategoryResponse?> GetByIdAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<List<RoomCategoryResponse>> GetAllAsync(CancellationToken cancellationToken = default);
}