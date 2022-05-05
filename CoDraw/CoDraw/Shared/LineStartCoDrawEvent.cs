using System.Text.Json.Serialization;

namespace CoDraw.Shared;

public class LineStartCoDrawEvent : LineCoDrawEvent, IEquatable<LineStartCoDrawEvent>
{
    public LineStartCoDrawEvent(long time, string stroke, List<Point> points) : base(
        CoDrawEventType.LineStartCoDrawEvent, time)
    {
        Stroke = stroke;
        Points = points;
    }

    [JsonPropertyName("stroke")] public string Stroke { get; }

    [JsonPropertyName("points")] public List<Point> Points { get; }

    #region IEquality

    public bool Equals(LineStartCoDrawEvent? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Stroke == other.Stroke && (Equals(Points, other.Points) || Points.SequenceEqual(other.Points));
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

        return Equals((LineStartCoDrawEvent)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Stroke, Points);
    }

    #endregion
}