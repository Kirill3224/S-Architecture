namespace SL.App.Interfaces;

public interface ISimulationService
{
    void ConductLesson(Guid teacherId, Guid groupId, string disciplineName, string activityName);
    int ConductExam(Guid groupId, string disciplineName);
}