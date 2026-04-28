using SL.Domain.Common;

namespace SL.Domain.Events;

public class LessonFinishedEventArgs : EventArgs
{
    public string DisciplineName { get; }
    public Activity CompletedActivity { get; }
    public DateTime Time { get; }

    public LessonFinishedEventArgs(string discipline, Activity activity)
    {
        DisciplineName = discipline;
        CompletedActivity = activity;
        Time = DateTime.Now;
    }
}