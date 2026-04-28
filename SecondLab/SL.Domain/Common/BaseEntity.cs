using System.Text.Json.Serialization;

namespace SL.Domain.Common;

public abstract class BaseEntity
{
    [JsonInclude]
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
}