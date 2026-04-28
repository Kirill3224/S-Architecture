

using SL.Domain.Entities;

namespace SL.App.Interfaces;

public interface IGroupService
{
    Guid Create(string name, int year);
    void AddStudentToGroup(Guid groupId, string firstName, string lastName);
    void Update(StudentGroup group);
    void Delete(Guid groupId);
    StudentGroup? GetById(Guid groupId);
    IEnumerable<StudentGroup> GetAll();
    IEnumerable<Student> GetAllStudentsInGroup(Guid groupId);
}