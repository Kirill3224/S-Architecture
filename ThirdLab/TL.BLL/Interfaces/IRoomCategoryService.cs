using TL.BLL.DTOs;

namespace TL.BLL.Interfaces;

public interface IRoomCategoryService
{
    Task<Guid> CreateAsync(CreateRoomCategoryRequest request);
    Task UpdateAsync(UpdateRoomCategoryRequest request);
    Task DeleteAsync(Guid categoryId);
    Task<RoomCategoryResponse?> GetByIdAsync(Guid categoryId);
    Task<List<RoomCategoryResponse>> GetAllAsync();
}