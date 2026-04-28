using SL.Domain.Interfaces;
using SL.Domain.Common;


namespace SL.Domain.Entities.Disciplines;

public class SoftwareArchitectureDiscipline : Discipline
{
    public SoftwareArchitectureDiscipline(ICourseMaterialsFactory factory) : base("Software Architecture", new() { 2 })
    {
        LoadCurriculum(factory);
    }
}