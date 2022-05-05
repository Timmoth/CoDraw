using System.Text.Json.Serialization;

namespace CoDraw.Shared;

public class LinePointsCoDrawEvent : LineCoDrawEvent, IEquatable<LinePointsCoDrawEvent>
{
    public LinePointsCoDrawEvent(long time, List<Point> points) : base(CoDrawEventType.LinePointsCoDrawEvent, time)
    {
        Points = points;
    }

    [JsonPropertyName("points")] public List<Point> Points { get; }

    public bool Equals(LinePointsCoDrawEvent? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Equals(Points, other.Points) || Points.SequenceEqual(other.Points);
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

        return Equals((LinePointsCoDrawEvent)obj);
    }

    public override int GetHashCode()
    {
        return Points.GetHashCode();
    }
}