using System.Text.Json.Serialization;

namespace CoDraw.Shared;

public class MouseMove : UserEvent, IEquatable<MouseMove>
{
    public MouseMove(List<float> points) : base(UserEventType.MouseMove)
    {
        Points = points;
    }

    [JsonPropertyName("p")] public List<float> Points { get; set; }

    public bool Equals(MouseMove? other)
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

    public List<Point> GetPoints()
    {
        var points = new List<Point>();
        for (var i = 0; i < Points.Count / 2; i += 2)
        {
            points.Add(new Point(Points[i], Points[i + 1]));
        }

        return points;
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

        return Equals((MouseMove)obj);
    }

    public override int GetHashCode()
    {
        return Points.GetHashCode();
    }
}