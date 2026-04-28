

namespace SL.Domain.Common;

public abstract class Equipment : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public bool IsBusy { get; private set; } = false;

    public Equipment(string name)
    {
        Name = name;
    }

    protected Equipment() { }

    public void SetBusy(bool newStatus)
    {
        IsBusy = newStatus;
    }
}