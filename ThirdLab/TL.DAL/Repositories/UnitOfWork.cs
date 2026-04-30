using TL.DAL.Entities;
using TL.DAL.Interfaces;
using TL.DAL.Persistence;
using System.Collections;

namespace TL.DAL.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private Hashtable _repositories = null!;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}