using SL.Domain.Interfaces;
using SL.Domain.Common;


namespace SL.Domain.Entities.Disciplines;

public class OOPDiscipline : Discipline
{
    public OOPDiscipline(ICourseMaterialsFactory factory) : base("Object Oriented Programming", new() { 1 })
    {
        LoadCurriculum(factory);
    }
}