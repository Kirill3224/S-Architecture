using SL.Domain.Common;
using SL.Domain.Events;

namespace SL.Domain.Entities;

public class Teacher : BaseEntity
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public bool IsBusy { get; private set; } = false;

    public event EventHandler<LessonFinishedEventArgs>? LessonFinished;
    public event EventHandler<LessonStartedEventArgs>? LessonStarted;

    public Teacher(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    protected Teacher() { }

    public void ConductLesson(Discipline discipline, Activity activity)
    {
        SetBusy(true);
        OnLessonStarted(new LessonStartedEventArgs(discipline.Name, activity));

        SetBusy(false);
        OnLessonFinished(new LessonFinishedEventArgs(discipline.Name, activity));
    }

    protected virtual void OnLessonFinished(LessonFinishedEventArgs e)
    {
        LessonFinished?.Invoke(this, e);
    }

    protected virtual void OnLessonStarted(LessonStartedEventArgs e)
    {
        LessonStarted?.Invoke(this, e);
    }

    public void SetBusy(bool newStatus)
    {
        IsBusy = newStatus;
    }

    public void UpdateInfo(string newFirstName, string newLastName)
    {
        if (string.IsNullOrWhiteSpace(newFirstName))
            throw new ArgumentException("First name cannot be empty");
        if (string.IsNullOrWhiteSpace(newLastName))
            throw new ArgumentException("Last name cannot be empty");

        FirstName = newFirstName;
        LastName = newLastName;
    }
}