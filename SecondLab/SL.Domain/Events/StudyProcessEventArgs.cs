namespace SL.Domain.Events;

public class StudyProcessEventArgs : EventArgs
{
    public string Message { get; }
    public bool IsError { get; }

    public StudyProcessEventArgs(string message, bool isError = false)
    {
        Message = message;
        IsError = isError;
    }
}