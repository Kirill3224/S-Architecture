using TL.DAL.Entities;

namespace TL.DAL.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}