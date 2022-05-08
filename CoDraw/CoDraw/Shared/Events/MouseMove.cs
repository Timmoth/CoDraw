using System.Text.Json.Serialization;
using Blazor.Extensions.Canvas.Canvas2D;

namespace CoDraw.Shared.Events;

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

    public override void Apply(UserState state)
    {
        var points = GetPoints();

        if (points.Count == 0)
        {
            return;
        }

        state.LastLinePoint = state.Point = points.Last();
    }

    public override async Task Apply(UserState state, Canvas2DContext context)
    {
        var points = GetPoints();

        if (points.Count == 0)
        {
            return;
        }

        if (state.MouseDown)
        {
            if (state.LastLinePoint.HasValue)
            {
                points.Insert(0, state.LastLinePoint.Value);
            }

            foreach (var point in points)
            {
                await context.LineToAsync(point.X, point.Y);
            }
        }
    }
}