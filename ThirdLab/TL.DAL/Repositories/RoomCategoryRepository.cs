using TL.DAL.Entities;
using TL.DAL.Interfaces;
using TL.DAL.Persistence;

namespace TL.DAL.Repositories;

public class RoomCategoryRepository : BaseRepository<RoomCategory>, IRoomCategoryRepository
{
    private RoomCategoryRepository(AppDbContext context) : base(context) { }
}