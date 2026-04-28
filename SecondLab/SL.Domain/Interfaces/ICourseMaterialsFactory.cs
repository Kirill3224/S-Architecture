using SL.Domain.Common;

namespace SL.Domain.Interfaces;

public interface ICourseMaterialsFactory
{
    List<Equipment> CreateEquipment();
    List<Activity> CreateActivities();
}