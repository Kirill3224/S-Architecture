using SL.Domain.Common;

namespace SL.Domain.Entities.Activities;

public class LabWork : Activity
{
    public LabWork() : base("Lab Work")
    {
        IsRequireSplit = true;
    }
}