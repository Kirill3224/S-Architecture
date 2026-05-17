using Microsoft.EntityFrameworkCore.Storage;
using TL.DAL.Interfaces;

namespace TL.DAL.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _currentTransaction;

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
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction != null)
            throw new InvalidOperationException("A transaction is already in progress.");

        _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            if (_currentTransaction != null)
            {
                await _currentTransaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackTransactionAsync(CancellationToken.None);
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync(cancellationToken);
            }
        }
        finally
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        await _context.DisposeAsync();

        GC.SuppressFinalize(this);
    }
}