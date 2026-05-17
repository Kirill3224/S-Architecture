using TL.BLL.DTOs;

namespace TL.BLL.Interfaces;

public interface IRoomService
{
    Task<Guid> CreateAsync(CreateRoomRequest request, CancellationToken cancellationToken = default);
    Task UpdateAsync(UpdateRoomRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid roomId, CancellationToken cancellationToken = default);
    Task<RoomResponse?> GetByIdAsync(Guid roomId, CancellationToken cancellationToken = default);
    Task<List<RoomResponse>> GetAllAsync(CancellationToken cancellationToken = default);
}