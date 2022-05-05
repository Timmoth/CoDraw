using System.Text.Json.Serialization;

namespace CoDraw.Shared;

public class CoDrawUserUpdateEvents : IEquatable<CoDrawUserUpdateEvents>
{
    public CoDrawUserUpdateEvents(Guid userId, List<UserCoDrawEvent> userEvents)
    {
        UserId = userId;
        UserEvents = userEvents;
    }

    [JsonPropertyName("userid")] public Guid UserId { get; set; }

    [JsonPropertyName("events")] public List<UserCoDrawEvent> UserEvents { get; set; }

    #region IEquatable

    public bool Equals(CoDrawUserUpdateEvents? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return UserId.Equals(other.UserId) && Equals(UserEvents, other.UserEvents) ||
               UserEvents.SequenceEqual(other.UserEvents);
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

        return Equals((CoDrawUserUpdateEvents)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(UserId, UserEvents);
    }

    #endregion
}