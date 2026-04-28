using SL.Domain.Common;
using SL.Domain.Entities;
using SL.Domain.Entities.Activities;
using SL.Domain.Interfaces;

namespace SL.Domain.Factories;

public class TechnicalCourseFactory : ICourseMaterialsFactory
{
    public List<Equipment> CreateEquipment()
    {
        return new List<Equipment>
        {
            new Computer()
        };
    }

    public List<Activity> CreateActivities()
    {
        return new List<Activity>
        {
            new Lecture(),
            new LabWork(),
            new TermPaper(),
            new ModularTests(),
            new Exam()
        };
    }
}