using SL.Domain.Common;

namespace SL.Domain.Events;

public class LessonStartedEventArgs : EventArgs
{
    public string DisciplineName { get; }
    public Activity StartedActivity { get; }
    public DateTime Time { get; }

    public LessonStartedEventArgs(string discipline, Activity activity)
    {
        DisciplineName = discipline;
        StartedActivity = activity;
        Time = DateTime.Now;
    }
}