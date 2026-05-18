using Microsoft.EntityFrameworkCore.Storage;
using TL.DAL.Interfaces;

namespace TL.DAL.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IRoomCategoryRepository Categories { get; }
    public IRoomRepository Rooms { get; }
    public IBookingRepository Bookings { get; }

    public UnitOfWork(AppDbContext context,
    IRoomRepository roomRepository,
    IRoomCategoryRepository categoryRepository,
    IBookingRepository bookingRepository)
    {
        _context = context;
        Categories = categoryRepository;
        Rooms = roomRepository;
        Bookings = bookingRepository;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var result = await _context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return result;
        }
        catch
        {
            await transaction.RollbackAsync(CancellationToken.None);
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}