using SL.Domain.Common;
using SL.Domain.Entities;
using SL.Domain.Entities.Activities;
using SL.Domain.Interfaces;

namespace SL.Domain.Factories;

public class LanguageCourseFactory : ICourseMaterialsFactory
{
    public List<Equipment> CreateEquipment()
    {
        return new List<Equipment>
        {
            new AudioSystem()
        };
    }

    public List<Activity> CreateActivities()
    {
        return new List<Activity>
        {
            new Practice(),
            new Test(),
            new Speaking(),
            new Reading(),
            new ModularTests(),
            new Credit()
        };
    }
}