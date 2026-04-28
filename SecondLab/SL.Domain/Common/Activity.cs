

namespace SL.Domain.Common;

public abstract class Activity : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public bool IsRequireSplit { get; protected set; } = false;
    public Activity(string name)
    {
        Name = name;
    }

    protected Activity() { }
}