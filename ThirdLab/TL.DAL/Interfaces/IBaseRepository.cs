using TL.DAL.Entities;

namespace TL.DAL.Interfaces;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<Guid> AddAsync(T item);
    void Update(T item);
    void Delete(T item);
}