

using System.ComponentModel.DataAnnotations;
using SL.App.Interfaces;
using SL.Domain.Entities;
using SL.Domain.Interfaces;

namespace SL.App.Services;

public class GroupService : IGroupService
{
    private readonly IRepository<StudentGroup> _repo;

    public GroupService(IRepository<StudentGroup> repo)
    {
        _repo = repo;
    }

    public Guid Create(string name, int year)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Name of group cannot be empty");

        if (year <= 0 || year > 5)
            throw new ValidationException("Invalid study year");


        var group = new StudentGroup(name, year);

        _repo.Add(group);

        return group.Id;
    }

    public void AddStudentToGroup(Guid groupId, string firstName, string lastName)
    {
        var group = GetById(groupId);

        if (group is null)
            throw new KeyNotFoundException($"Group with ID {groupId} not found.");

        var student = new Student(firstName, lastName);

        group.AddStudent(student);
        _repo.Update(group);
    }

    public void Update(StudentGroup group)
    {
        if (group is null)
            throw new KeyNotFoundException("Group not found");
        _repo.Update(group);
    }

    public void Delete(Guid groupId)
    {
        _repo.Delete(groupId);
    }

    public StudentGroup? GetById(Guid groupId)
    {
        return _repo.GetById(groupId);
    }

    public IEnumerable<StudentGroup> GetAll()
    {
        return _repo.GetAll();
    }

    public IEnumerable<Student> GetAllStudentsInGroup(Guid groupId)
    {
        var group = GetById(groupId);

        if (group is null)
            throw new KeyNotFoundException($"Group with ID {groupId} not found.");

        return group.Students;
    }
}