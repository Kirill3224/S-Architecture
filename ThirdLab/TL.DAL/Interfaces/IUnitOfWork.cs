using TL.DAL.Entities;

namespace TL.DAL.Interfaces;

public interface IUnitOfWork
{
    public IBaseRepository<T> Repository<T>() where T : BaseEntity;
    Task<int> SaveChangesAsync();
}