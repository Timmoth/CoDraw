using System.Text.Json.Serialization;

namespace CoDraw.Shared;

public class CoDrawLineUpdateEvents : IEquatable<CoDrawLineUpdateEvents>
{
    public CoDrawLineUpdateEvents(Guid userId, Dictionary<Guid, List<LineCoDrawEvent>> lineEvents)
    {
        UserId = userId;
        LineEvents = lineEvents;
    }

    [JsonPropertyName("userid")] public Guid UserId { get; set; }

    [JsonPropertyName("events")] public Dictionary<Guid, List<LineCoDrawEvent>> LineEvents { get; set; }

    public bool Equals(CoDrawLineUpdateEvents? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return UserId.Equals(other.UserId) &&
               (
                   Equals(LineEvents, other.LineEvents) || LineEvents.All(x =>
                       other.LineEvents.ContainsKey(x.Key) && x.Value.SequenceEqual(other.LineEvents[x.Key])));
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

        return Equals((CoDrawLineUpdateEvents)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(UserId, LineEvents);
    }

    public void Add(Guid lineId, LineCoDrawEvent lineEvent)
    {
        if (!LineEvents.TryGetValue(lineId, out var usersLines))
        {
            usersLines = LineEvents[lineId] = new List<LineCoDrawEvent>();
        }

        usersLines.Add(lineEvent);
    }
}