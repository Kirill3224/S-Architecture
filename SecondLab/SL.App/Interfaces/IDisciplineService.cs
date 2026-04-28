using SL.Domain.Common;

namespace SL.App.Interfaces;

public interface IDisciplineService
{
    IEnumerable<Discipline> GetAll();
    public Discipline? GetByName(string name);
}