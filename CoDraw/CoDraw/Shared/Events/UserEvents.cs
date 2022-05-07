using System.Text.Json.Serialization;

namespace CoDraw.Shared;

public class UserEvents : IEquatable<UserEvents>
{
    public UserEvents(Guid userId, List<UserEvent> events)
    {
        UserId = userId;
        Events = events;
    }

    [JsonPropertyName("u")] public Guid UserId { get; set; }

    [JsonPropertyName("e")] public List<UserEvent> Events { get; set; }

    public bool Equals(UserEvents? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return UserId.Equals(other.UserId) && (Equals(Events, other.Events) || Events.SequenceEqual(other.Events));
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        return Equals((UserEvents)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(UserId, Events);
    }
}