using SL.Domain.Common;
using SL.Domain.Interfaces;

namespace SL.Domain.Entities.Disciplines;

public class EnglishDiscipline : Discipline
{
    public EnglishDiscipline(ICourseMaterialsFactory factory) : base("English", new() { 1, 2 })
    {
        LoadCurriculum(factory);
    }
}