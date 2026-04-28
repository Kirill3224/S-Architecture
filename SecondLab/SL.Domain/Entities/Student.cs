using SL.Domain.Common;

namespace SL.Domain.Entities;

public class Student : BaseEntity
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public List<string> CompletedWorks { get; } = new();

    public Student(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    protected Student() { }
}
