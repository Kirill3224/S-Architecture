using SL.App.Interfaces;
using SL.Domain.Common;
using SL.Domain.Entities.Disciplines;
using SL.Domain.Factories;

namespace SL.App.Services;

public class DisciplineService : IDisciplineService
{
    private readonly List<Discipline> _disciplines;

    public DisciplineService(TechnicalCourseFactory techFactory, LanguageCourseFactory langFactory)
    {
        _disciplines = new List<Discipline>
        {
          new OOPDiscipline(techFactory),
          new SoftwareArchitectureDiscipline(techFactory),
          new EnglishDiscipline(langFactory)
        };
    }

    public IEnumerable<Discipline> GetAll()
    {
        return _disciplines;
    }

    public Discipline? GetByName(string name)
    {
        return _disciplines.FirstOrDefault(d => d.Name == name);
    }
}