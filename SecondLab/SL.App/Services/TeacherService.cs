using SL.App.Interfaces;
using SL.Domain.Interfaces;
using SL.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace SL.App.Services;

public class TeacherService : ITeacherService
{
    private readonly IRepository<Teacher> _repo;

    public TeacherService(IRepository<Teacher> repo)
    {
        _repo = repo;
    }

    public Guid Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ValidationException("Teacher first name cannot be empty");
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ValidationException("Teacher last name cannot be empty");

        var teacher = new Teacher(firstName, lastName);
        _repo.Add(teacher);

        return teacher.Id;
    }

    public void Update(Teacher teacher)
    {
        if (teacher is null)
            throw new KeyNotFoundException("Teacher not found");

        _repo.Update(teacher);
    }

    public void Delete(Guid teacherId)
    {
        _repo.Delete(teacherId);
    }

    public Teacher? GetById(Guid teacherId)
    {
        return _repo.GetById(teacherId);
    }

    public IEnumerable<Teacher> GetAll()
    {
        return _repo.GetAll();
    }
}