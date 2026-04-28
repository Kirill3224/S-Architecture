

using SL.Domain.Entities;

namespace SL.App.Interfaces;

public interface ITeacherService
{
    Guid Create(string firstName, string secondName);
    void Update(Teacher teacher);
    void Delete(Guid teacherId);
    Teacher? GetById(Guid teacherId);
    IEnumerable<Teacher> GetAll();
}