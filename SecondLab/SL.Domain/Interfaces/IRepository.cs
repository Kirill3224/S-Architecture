

using SL.Domain.Common;

namespace SL.Domain.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    IEnumerable<T> GetAll();
    T? GetById(Guid id);
    Guid Add(T item);
    void Delete(Guid id);
    void Update(T item);

}