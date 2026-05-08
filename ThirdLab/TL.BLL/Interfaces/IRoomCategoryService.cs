using TL.BLL.DTOs;

namespace TL.BLL.Interfaces;

public interface IRoomCategoryService
{
    Task<Guid> CreateAsync(CreateRoomCategoryRequest request);
    Task UpdateAsync(UpdateRoomCategoryRequest request);
    Task DeleteAsync(Guid roomCategoryId);
    Task<RoomCategoryResponse?> GetByIdAsync(Guid roomCategoryId);
    Task<List<RoomCategoryResponse>> GetAllAsync();
}