using SL.Domain.Entities;
using SL.Domain.Interfaces;

namespace SL.Domain.Common;

public abstract class Discipline : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public List<int> AllowedYears { get; } = new();
    private readonly List<Activity> _activities = new();
    private readonly List<Equipment> _requiredEquipment = new();

    public Discipline(string name, List<int> allowedYears)
    {
        Name = name;
        AllowedYears = allowedYears;
    }

    protected Discipline() { }

    public IReadOnlyList<Activity> Activities => _activities;
    public IReadOnlyList<Equipment> RequiredEquipment => _requiredEquipment;

    public bool CanBeStudiedBy(StudentGroup group)
    {
        return AllowedYears.Contains(group.Year);
    }

    public void LoadCurriculum(ICourseMaterialsFactory factory)
    {
        _activities.AddRange(factory.CreateActivities());
        _requiredEquipment.AddRange(factory.CreateEquipment());
    }
}