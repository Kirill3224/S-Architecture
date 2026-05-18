namespace TL.DAL.Interfaces;

public interface IUnitOfWork : IAsyncDisposable
{
    IRoomCategoryRepository Categories { get; }
    IRoomRepository Rooms { get; }
    IBookingRepository Bookings { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}