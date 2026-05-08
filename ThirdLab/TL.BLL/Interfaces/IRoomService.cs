using TL.BLL.DTOs;

namespace TL.BLL.Interfaces;

public interface IRoomService
{
    Task<Guid> CreateAsync(CreateRoomRequest request);
    Task UpdateAsync(UpdateRoomRequest request);
    Task DeleteAsync(Guid roomId);
    Task<RoomResponse?> GetRoomByIdAsync(Guid roomId);
    Task<List<RoomResponse>> GetAllAsync();
}