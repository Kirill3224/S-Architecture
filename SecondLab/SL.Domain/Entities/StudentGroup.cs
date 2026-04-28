using System.Text.Json.Serialization;
using SL.Domain.Common;
using SL.Domain.Events;

namespace SL.Domain.Entities;

public class StudentGroup : BaseEntity
{
    private static readonly Random _random = new();
    public string Name { get; private set; } = string.Empty;
    public int Year { get; private set; }
    public bool IsBusy { get; private set; } = false;

    [JsonInclude]
    public List<Student> Students { get; private set; } = new();

    [JsonInclude]
    public List<string> CompletedWorks { get; private set; } = new();

    [JsonInclude]
    public Dictionary<string, int> GradeBook { get; private set; } = new();

    public int Size => Students.Count;
    public int SubgroupSize => Size / 2;

    public event EventHandler<LessonFinishedEventArgs>? JournalUpdated;

    public StudentGroup(string name, int year)
    {
        Name = name;
        Year = year;
    }

    protected StudentGroup() { }

    public void AddStudent(Student student) => Students.Add(student);

    public void OnLessonStarted(object? sender, LessonStartedEventArgs e) => SetBusy(true);

    public void OnLessonFinished(object? sender, LessonFinishedEventArgs e)
    {
        string workRecord = $"{e.DisciplineName}: {e.CompletedActivity.Name}";

        CompletedWorks.Add(workRecord);

        foreach (var student in Students)
        {
            student.CompletedWorks.Add(workRecord);
        }

        SetBusy(false);
        OnJournalUpdated(e);
    }

    public void SetBusy(bool newStatus) => IsBusy = newStatus;

    public void TakeExam(string disciplineName)
    {
        int attendanceBonus = CompletedWorks.Count(w => w.StartsWith(disciplineName)) * 5;
        int luck = _random.Next(0, 21);

        int totalScore = Math.Min(50 + attendanceBonus + luck, 100);

        GradeBook[disciplineName] = totalScore;
    }

    public void TakeCredit(string disciplineName)
    {
        if (CompletedWorks.Any(w => w.StartsWith(disciplineName)))
        {
            GradeBook[disciplineName] = 100;
        }
    }

    public void UpdateInfo(string newName, int newYear)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Group name cannot be empty");

        if (newYear > 5 || newYear <= 0)
            throw new ArgumentException($"Invalid year {newYear}. Must be 1-6.");

        Name = newName;
        Year = newYear;
    }

    protected virtual void OnJournalUpdated(LessonFinishedEventArgs e)
    {
        JournalUpdated?.Invoke(this, e);
    }
}