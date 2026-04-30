using TL.DAL.Entities;
using TL.DAL.Interfaces;
using TL.DAL.Persistence;

namespace TL.DAL.Repositories;

public class RoomCategoryRepository : BaseRepository<RoomCategory>, IRoomCategoryRepository
{
    public RoomCategoryRepository(AppDbContext context) : base(context) { }
}